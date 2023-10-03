namespace API_Identity.Entities;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Value { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
