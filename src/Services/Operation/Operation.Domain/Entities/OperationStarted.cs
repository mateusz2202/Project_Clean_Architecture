using Operation.Domain.Contracts;

namespace Operation.Domain.Entities;

public partial class OperationStarted : AuditableEntity<int>
{
    public int Id { get; set; }

    public int OperationPlannedId { get; set; }

    public DateTime StartDate { get; set; }

    public virtual OperationPlanned OperationPlanned { get; set; } = null!;
}

