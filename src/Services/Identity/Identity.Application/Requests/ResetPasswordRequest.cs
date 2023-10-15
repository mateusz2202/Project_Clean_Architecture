using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Requests;

public class ResetPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
    [Required]
    public string Token { get; set; } = string.Empty;
}
