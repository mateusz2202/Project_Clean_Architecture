namespace Operation.Domain.Contracts;

public interface IAuditableEntity<TId> : IAuditableEntity, IEntity<TId>
{
}

public interface IAuditableEntity : IEntity
{
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
}
