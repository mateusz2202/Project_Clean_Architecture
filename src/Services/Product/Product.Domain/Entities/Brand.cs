using Product.Domain.Contracts;

namespace Product.Domain.Entities;

public class Brand : AuditableEntity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Tax { get; set; }
}
