using Identity.Shared.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Identity.Shared.Models;

public class ApplicationRoleClaim : IdentityRoleClaim<string>, IAuditableEntity<int>
{
    
    public string Description { get; set; }
    public string Group { get; set; }
    public virtual ApplicationRole Role { get; set; }    
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
    public DateTime? ModifiedOn { get; set; }
}
