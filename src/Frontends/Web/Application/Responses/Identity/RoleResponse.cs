using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Application.Responses.Identity;

public class RoleResponse
{
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
}