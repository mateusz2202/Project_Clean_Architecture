using Identity.Shared.Contracts;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Shared.Models;

public class ApplicationUser : IdentityUser<string>, IAuditableEntity<string>
{
    public string FirstName { get; set; } 
    public string LastName { get; set; } 

    [Column(TypeName = "text")]
    public string? ProfilePictureDataUrl { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
    public bool IsActive { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? ModifiedBy { get; set; } 
    public DateTime? ModifiedOn { get; set; }
}

