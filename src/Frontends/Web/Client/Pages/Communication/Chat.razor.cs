using BlazorApp.Client.Extensions;
using BlazorApp.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System.Threading.Tasks;
using BlazorApp.Shared.Constants.Storage;

namespace BlazorApp.Client.Pages.Communication;

public partial class Chat
{

    [CascadingParameter] private HubConnection HubConnection { get; set; }
    [Parameter] public string CurrentMessage { get; set; }
    [Parameter] public string CurrentUserId { get; set; }
    [Parameter] public string CurrentUserImageURL { get; set; }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
    }

    private async Task SubmitAsync()
    {
        await Task.CompletedTask;
    }

    private async Task OnKeyPressInChat(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await SubmitAsync();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
        if (HubConnection.State == HubConnectionState.Disconnected)
        {
            await HubConnection.StartAsync();
        }  
        await GetUsersAsync();
        var state = await _stateProvider.GetAuthenticationStateAsync();
        var user = state.User;
        CurrentUserId = user.GetUserId();
        CurrentUserImageURL = await _localStorage.GetItemAsync<string>(StorageConstants.Local.UserImageURL);
        if (!string.IsNullOrEmpty(CId))
        {
            await LoadUserChat(CId);
        }

        await HubConnection.SendAsync(ApplicationConstants.SignalR.PingRequest, CurrentUserId);
    }

    [Parameter] public string CFullName { get; set; }
    [Parameter] public string CId { get; set; }
    [Parameter] public string CUserName { get; set; }
    [Parameter] public string CImageURL { get; set; }

    private async Task LoadUserChat(string userId)
    {
        _open = false;
        var response = await _userManager.GetAsync(userId);
        if (response.Succeeded)
        {
            var contact = response.Data;
            CId = contact.Id;
            CFullName = $"{contact.FirstName} {contact.LastName}";
            CUserName = contact.UserName;
            CImageURL = contact.ProfilePictureDataUrl;
            _navigationManager.NavigateTo($"chat/{CId}");

        }
        else
        {
            foreach (var message in response.Messages)
            {
                _snackBar.Add(message, Severity.Error);
            }
        }
    }

    private async Task GetUsersAsync()
    {
        await Task.CompletedTask;
    }

    private bool _open;
    private Anchor ChatDrawer { get; set; }

    private void OpenDrawer(Anchor anchor)
    {
        ChatDrawer = anchor;
        _open = true;
    }

    private Color GetUserStatusBadgeColor(bool isOnline)
    {
        switch (isOnline)
        {
            case false:
                return Color.Error;
            case true:
                return Color.Success;
        }
    }
}