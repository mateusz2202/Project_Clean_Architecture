using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Application.Requests.Identity
{
    public class RoleRequest
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}