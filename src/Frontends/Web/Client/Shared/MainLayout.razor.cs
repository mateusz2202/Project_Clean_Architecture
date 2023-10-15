using BlazorApp.Client.Infrastructure.Settings;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace BlazorApp.Client.Shared
{
    public partial class MainLayout : IDisposable
    {
        private MudTheme _currentTheme;
        private bool _rightToLeft = false;
        private async Task RightToLeftToggle(bool value)
        {
            _rightToLeft = value;
            await Task.CompletedTask;
        }

        protected override async Task OnInitializedAsync()
        {
            _currentTheme = BlazorAppTheme.DefaultTheme;
            _currentTheme = await _clientPreferenceManager.GetCurrentThemeAsync();
            _rightToLeft = await _clientPreferenceManager.IsRTL();
            _interceptor.RegisterEvent();
        }

        private async Task DarkMode()
        {
            bool isDarkMode = await _clientPreferenceManager.ToggleDarkModeAsync();
            _currentTheme = isDarkMode
                ? BlazorAppTheme.DefaultTheme
                : BlazorAppTheme.DarkTheme;
        }

        public void Dispose()
        {
            _interceptor.DisposeEvent();
        }
    }
}