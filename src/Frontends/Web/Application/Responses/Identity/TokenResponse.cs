using System;

namespace BlazorHero.CleanArchitecture.Application.Responses.Identity
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UserImageURL { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }

    public class AuthenticationResponse
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }

}