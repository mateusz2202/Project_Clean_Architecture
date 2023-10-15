using System.ComponentModel.DataAnnotations;


namespace Identity.Application.Requests;

public class ChangePasswordRequest
{
    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
