namespace BlazorApp.Client.Infrastructure.Routes
{
    public static class TokenEndpoints
    {
        public static string Get = "api/identity/account/authenticate";
        public static string Refresh = "api/identity/token/refresh";
    }
}