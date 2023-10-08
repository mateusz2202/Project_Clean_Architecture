using System.Linq.Expressions;

namespace Operation.Application.Contracts.Repositories;

public interface IOperationRepository
{
    public Task<bool> IsCodeUsed(string code);
    public Task<bool> Any(Expression<Func<Domain.Entities.Operation, bool>> condtion);
    public Task<Domain.Entities.Operation?> GetByCode(string code);
}
