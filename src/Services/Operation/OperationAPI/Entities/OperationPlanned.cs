using System;
using System.Collections.Generic;

namespace OperationAPI.Entities;

public partial class OperationPlanned
{
    public int Id { get; set; }

    public int OperationId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual Operation Operation { get; set; } = null!;

    public virtual ICollection<OperationStarted> OperationStarteds { get; set; } = new List<OperationStarted>();
}
