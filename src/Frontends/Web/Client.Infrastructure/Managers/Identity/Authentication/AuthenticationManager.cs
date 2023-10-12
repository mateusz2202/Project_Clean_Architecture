﻿using Blazored.LocalStorage;
using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Authentication;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Routes;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Constants.Storage;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;


namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Identity.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IStringLocalizer<AuthenticationManager> _localizer;

        public AuthenticationManager(
            IHttpClientFactory httpClientFactory,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider,
            IStringLocalizer<AuthenticationManager> localizer)
        {
            _httpClientFactory = httpClientFactory;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            _localizer = localizer;
        }

        public async Task<ClaimsPrincipal> CurrentUser()
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return state.User;
        }

        public async Task<IResult> Login(TokenRequest model)
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);

            var response = await httpClient.PostAsJsonAsync(TokenEndpoints.Get, model);
      
            var result = await response.ToResult<TokenResponse>();

            if (result.Succeeded)
            {
                var token = result.Data.Token;
                var refreshToken = result.Data.RefreshToken;
                var userImageURL = result.Data.UserImageURL;
                await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
                await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);
                if (!string.IsNullOrEmpty(userImageURL))
                {
                    await _localStorage.SetItemAsync(StorageConstants.Local.UserImageURL, userImageURL);
                }

                await ((BlazorHeroStateProvider)this._authenticationStateProvider).StateChangedAsync();

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await Result.SuccessAsync();
            }
            else
            {
                return await Result.FailAsync(result.Messages);
            }
        }

        public async Task<IResult> Logout()
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
            await _localStorage.RemoveItemAsync(StorageConstants.Local.AuthToken);
            await _localStorage.RemoveItemAsync(StorageConstants.Local.RefreshToken);
            await _localStorage.RemoveItemAsync(StorageConstants.Local.UserImageURL);
            ((BlazorHeroStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            httpClient.DefaultRequestHeaders.Authorization = null;
            return await Result.SuccessAsync();
        }

        public async Task<string> RefreshToken()
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);

            var token = await _localStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);
            var refreshToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.RefreshToken);

            var response = await httpClient.PostAsJsonAsync(Routes.TokenEndpoints.Refresh, new RefreshTokenRequest { Token = token, RefreshToken = refreshToken });

            var result = await response.ToResult<TokenResponse>();

            if (!result.Succeeded)
            {
                throw new ApplicationException(_localizer["Something went wrong during the refresh token action"]);
            }

            token = result.Data.Token;
            refreshToken = result.Data.RefreshToken;
            await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
            await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return token;
        }

        public async Task<string> TryRefreshToken()
        {
            var availableToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.RefreshToken);
            if (string.IsNullOrEmpty(availableToken)) return string.Empty;
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            var timeUTC = DateTime.UtcNow;
            var diff = expTime - timeUTC;
            if (diff.TotalMinutes <= 1)
                return await RefreshToken();
            return string.Empty;
        }

        public async Task<string> TryForceRefreshToken()
        {
            return await RefreshToken();
        }
    }
}