using MediatR;
using Microsoft.Azure.Cosmos;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Contracts.Services;
using Operation.Shared.Constans;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Commands.DeleteOperation;

public record DeleteOperationByCodeCommand(string Code) : IRequest<Result>;

public class DeleteOperationByCodeCommandCommandHandler : IRequestHandler<DeleteOperationByCodeCommand, Result>
{
    private readonly ICosmosService _cosmosService;
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IOperationRepository _operationRepository;
    public DeleteOperationByCodeCommandCommandHandler(   
        IUnitOfWork<int> unitOfWork,
        IOperationRepository operationRepository,
        ICosmosService cosmosService)
    {       
        _unitOfWork = unitOfWork;
        _operationRepository = operationRepository;
        _cosmosService = cosmosService;
    }

    public async Task<Result> Handle(DeleteOperationByCodeCommand request, CancellationToken cancellationToken)
    {

        var operationToDelete = await _operationRepository.GetByCode(request.Code);
        if(operationToDelete == null) 
            return (Result)await Result.FailAsync();

        await _cosmosService
                .Delete(
                     containerName: ApplicationConstants.CosmosDB.CONTAINER_OPERATION,
                     id: operationToDelete.Id.ToString(),
                     partitionKey: new PartitionKey(operationToDelete.Code),
                     cancellationToken: cancellationToken);

        await _unitOfWork.Repository<Domain.Entities.Operation>().DeleteAsync(operationToDelete);

        await _unitOfWork.CommitAndRemoveCache(cancellationToken,
                                   new string[]
                                   {    ApplicationConstants.Cache.OPERATION_KEY ,
                                        ApplicationConstants.Cache.OPERATIONATTRIBUTE_KEY,
                                        ApplicationConstants.Cache.OPERATIONWITHATTRIBUTE_KEY,
                                   });

        return (Result)await Result.SuccessAsync();
    }

}
