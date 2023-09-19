using System;
using System.Collections.Generic;

namespace OperationAPI.Entities;

public partial class Operation
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<OperationPlanned> OperationPlanneds { get; set; } = new List<OperationPlanned>();
}
