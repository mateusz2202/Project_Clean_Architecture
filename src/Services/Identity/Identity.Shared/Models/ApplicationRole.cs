using Identity.Shared.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Identity.Shared.Models;

public class ApplicationRole : IdentityRole, IAuditableEntity
{
    public string Description { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
    public DateTime? ModifiedOn { get; set; }
}
