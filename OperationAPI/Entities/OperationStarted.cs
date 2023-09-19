using System;
using System.Collections.Generic;

namespace OperationAPI.Entities;

public partial class OperationStarted
{
    public int Id { get; set; }

    public int OperationPlannedId { get; set; }

    public DateTime StartDate { get; set; }

    public virtual OperationPlanned OperationPlanned { get; set; } = null!;
}
