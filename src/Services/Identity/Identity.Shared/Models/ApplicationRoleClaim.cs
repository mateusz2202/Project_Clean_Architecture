using Identity.Shared.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Identity.Shared.Models;

public class ApplicationRoleClaim : IdentityRoleClaim<string>, IAuditableEntity<int>
{   
    public string? Description { get; set; }
    public string? Group { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? ModifiedBy { get; set; } 
    public DateTime? ModifiedOn { get; set; }

    public virtual ApplicationRole Role { get; set; }

    public ApplicationRoleClaim() : base()
    {
    }

    public ApplicationRoleClaim(string roleClaimDescription = null, string roleClaimGroup = null) : base()
    {
        Description = roleClaimDescription;
        Group = roleClaimGroup;
    }
}
