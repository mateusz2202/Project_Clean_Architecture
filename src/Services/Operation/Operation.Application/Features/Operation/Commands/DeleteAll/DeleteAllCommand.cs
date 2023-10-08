using MediatR;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Contracts.Services;
using Operation.Shared.Constans;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Commands.DeleteAll;

public record DeleteAllCommand : IRequest<Result>;

public class DeleteAllCommandHandler : IRequestHandler<DeleteAllCommand, Result>
{
    private readonly ICosmosService _cosmosService;
    private readonly IUnitOfWork<int> _unitOfWork;
    public DeleteAllCommandHandler(
        IUnitOfWork<int> unitOfWork,
        ICosmosService cosmosService)
    {
        _unitOfWork = unitOfWork;
        _cosmosService = cosmosService;
    }

    public async Task<Result> Handle(DeleteAllCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.Repository<Domain.Entities.Operation>().DeleteAsync(x => true);
        await _cosmosService.DeleteAll(containerName: ApplicationConstants.CosmosDB.CONTAINER_OPERATION, cancellationToken);
        return (Result)await Result.SuccessAsync();
    }

}
