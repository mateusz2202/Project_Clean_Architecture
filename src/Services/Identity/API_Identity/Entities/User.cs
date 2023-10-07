namespace API_Identity.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public int RoleId { get; set; }

    public string Password { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public virtual Role Role { get; set; } = null!;
}
