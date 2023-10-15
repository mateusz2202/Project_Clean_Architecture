using Identity.Shared.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Identity.Shared.Models;

public class ApplicationRole : IdentityRole, IAuditableEntity<string>
{  
    public string? Description { get; set; } 
    public string? CreatedBy { get; set; } 
    public DateTime CreatedOn { get; set; }
    public string? ModifiedBy { get; set; } 
    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }

    public ApplicationRole() : base()
    {
        RoleClaims = new HashSet<ApplicationRoleClaim>();
    }
    public ApplicationRole(string roleName, string roleDescription = null) : base(roleName)
    {
        RoleClaims = new HashSet<ApplicationRoleClaim>();
        Description = roleDescription;
    }
}
