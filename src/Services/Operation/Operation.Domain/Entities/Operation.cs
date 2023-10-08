using Operation.Domain.Contracts;

namespace Operation.Domain.Entities;

public partial class Operation : AuditableEntity<int>
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<OperationPlanned> OperationPlanneds { get; set; } = new List<OperationPlanned>();
}
