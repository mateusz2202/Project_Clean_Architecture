using BlazorApp.Shared.Wrapper;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorApp.Client.Infrastructure.Extensions;

internal static class ResultExtensions
{
    internal static async Task<IResult<T>> ToResult<T>(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<Result<T>>(responseAsString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        });
        return responseObject;
    }

    internal static async Task<IResult> ToResult(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<Result>(responseAsString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        });
        return responseObject;
    }

    internal static async Task<PaginatedResult<T>> ToPaginatedResult<T>(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<PaginatedResult<T>>(responseAsString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return responseObject;
    }


    internal static async Task<IResult<T>> GetResult<T>(this HttpClient httpClient, string uriString)
    {
        var response = await httpClient
            .SendAsync(new HttpRequestMessage
            (
                method: HttpMethod.Get,
                requestUri: new Uri(uriString, UriKind.Relative)
            ));
        return await response.ToResult<T>();
    }

    internal static async Task<PaginatedResult<T>> GetPaginatedResult<T>(this HttpClient httpClient, string uriString)
    {
        var response = await httpClient
            .SendAsync(new HttpRequestMessage
            (
                method: HttpMethod.Get,
                requestUri: new Uri(uriString, UriKind.Relative)
            ));
        return await response.ToPaginatedResult<T>();
    }

    internal static async Task<IResult<T>> DeleteResult<T>(this HttpClient httpClient, string uriString)
    {
        var response = await httpClient
            .SendAsync(new HttpRequestMessage
            (
                method: HttpMethod.Delete,
                requestUri: new Uri(uriString, UriKind.Relative)
            ));
        return await response.ToResult<T>();
    }

    internal static async Task<IResult<T>> PostAsJsonResult<T, V>(this HttpClient httpClient, string uriString, V value)
    {
        var response = await httpClient.PostAsJsonAsync(uriString, value);
        return await response.ToResult<T>();
    }

    internal static async Task<IResult<T>> PuttAsJsonResult<T, V>(this HttpClient httpClient, string uriString, V value)
    {
        var response = await httpClient.PutAsJsonAsync(uriString, value);
        return await response.ToResult<T>();
    }

}