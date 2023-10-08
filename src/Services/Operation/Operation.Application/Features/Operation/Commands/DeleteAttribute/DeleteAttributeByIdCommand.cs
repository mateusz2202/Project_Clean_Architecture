using MediatR;
using Microsoft.Azure.Cosmos;
using Operation.Application.Common.Exceptions;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Contracts.Services;
using Operation.Shared.Constans;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Commands.DeleteAttribute;

public record DeleteAttributeByIdCommand(int Id) : IRequest<Result>;

public class DeleteAttributeByIdCommandHandler : IRequestHandler<DeleteAttributeByIdCommand, Result>
{
    private readonly ICosmosService _cosmosService;
    private readonly IUnitOfWork<int> _unitOfWork;
    public DeleteAttributeByIdCommandHandler(
        IUnitOfWork<int> unitOfWork,
        ICosmosService cosmosService)
    {
        _unitOfWork = unitOfWork;
        _cosmosService = cosmosService;
    }

    public async Task<Result> Handle(DeleteAttributeByIdCommand request, CancellationToken cancellationToken)
    {

        var operationToDelete = await _unitOfWork.Repository<Domain.Entities.Operation>()
                                                 .GetByIdAsync(request.Id)
                                                 ?? throw new NotFoundException("not found operation");

        await _cosmosService
                .Delete(
                    containerName: ApplicationConstants.CosmosDB.CONTAINER_OPERATION,
                    id: operationToDelete.Id.ToString(),
                    partitionKey: new PartitionKey(operationToDelete.Code),
                    cancellationToken: cancellationToken);

        return (Result)await Result.SuccessAsync();
    }

}
