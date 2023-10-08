using MediatR;
using Microsoft.Azure.Cosmos;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Contracts.Services;
using Operation.Shared.Constans;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Commands.DeleteOperation;

public record DeleteOperationByIdCommand : IRequest<Result>
{
    public int Id { get; set; }
}

public class DeleteOperationByIdCommandHandler : IRequestHandler<DeleteOperationByIdCommand, Result>
{
    private readonly IOperationService _operationService;
    private readonly IUnitOfWork<int> _unitOfWork;
    public DeleteOperationByIdCommandHandler( 
        IOperationService operationService,
        IUnitOfWork<int> unitOfWork)
    {      
        _operationService = operationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteOperationByIdCommand request, CancellationToken cancellationToken)
    {

        var operationToDelete = await _unitOfWork.Repository<Domain.Entities.Operation>().GetByIdAsync(request.Id);

        await _operationService.DeleteAttribute(operationToDelete.Id.ToString(), new PartitionKey(operationToDelete.Code), cancellationToken);

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