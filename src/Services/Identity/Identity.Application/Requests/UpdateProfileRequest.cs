using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Requests;

public class UpdateProfileRequest
{
    [Required]
    public string FirstName { get; set; }=string.Empty;

    [Required]
    public string LastName { get; set; }=string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; }=string.Empty;
}
