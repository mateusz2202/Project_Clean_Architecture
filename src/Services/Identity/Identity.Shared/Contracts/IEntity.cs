namespace Identity.Shared.Contracts;

public interface IEntity<TId> : IEntity
{
    public TId Id { get; set; }
}

public interface IEntity
{
}
