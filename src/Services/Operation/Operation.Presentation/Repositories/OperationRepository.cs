using Microsoft.EntityFrameworkCore;
using Operation.Application.Contracts.Repositories;
using System.Linq.Expressions;

namespace Operation.Persistence.Repositories;

public class OperationRepository : IOperationRepository
{
    private readonly IRepositoryAsync<Domain.Entities.Operation, int> _repository;

    public OperationRepository(IRepositoryAsync<Domain.Entities.Operation, int> repository)
    {
        _repository = repository;
    }

    public async Task<bool> IsCodeUsed(string code)
        => await _repository.Entities.AnyAsync(x => x.Code.ToLower() == code.ToLower());

    public async Task<bool> Any(Expression<Func<Domain.Entities.Operation, bool>> condtion)
        => await _repository.Entities.AnyAsync(condtion);

    public async Task<Domain.Entities.Operation?> GetByCode(string code)
         => await _repository.Entities.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower());


}
