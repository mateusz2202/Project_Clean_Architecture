using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Requests;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
