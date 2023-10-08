using MediatR;
using Microsoft.Azure.Cosmos;
using Operation.Application.Common.Exceptions;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Contracts.Services;
using Operation.Shared.Constans;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Commands.DeleteAttribute;

public record DeleteAttributeByCodeCommand(string Code) : IRequest<Result>;

public class DeleteAttributeByCodeCommandHandler : IRequestHandler<DeleteAttributeByCodeCommand, Result>
{
    private readonly ICosmosService _cosmosService;
    private readonly IOperationRepository _operationRepository;
    public DeleteAttributeByCodeCommandHandler(
        IOperationRepository operationRepository,
        ICosmosService cosmosService)
    {
        _operationRepository = operationRepository;
        _cosmosService = cosmosService;
    }

    public async Task<Result> Handle(DeleteAttributeByCodeCommand request, CancellationToken cancellationToken)
    {
        var operationToDelete = await _operationRepository.GetByCode(request.Code)
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
