using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Localization;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Contracts.Services;
using Operation.Shared.Constans;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Commands.DeleteOperation;

public record DeleteOperationByCodeCommand : IRequest<Result>
{
    public string Code { get; set; } = string.Empty;
}

public class DeleteOperationByCodeCommandCommandHandler : IRequestHandler<DeleteOperationByCodeCommand, Result>
{
    private readonly IOperationService _operationService;
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IOperationRepository _operationRepository;
    public DeleteOperationByCodeCommandCommandHandler(    
        IOperationService operationService,
        IUnitOfWork<int> unitOfWork,
        IOperationRepository operationRepository)
    {        
        _operationService = operationService;
        _unitOfWork = unitOfWork;
        _operationRepository = operationRepository;
    }

    public async Task<Result> Handle(DeleteOperationByCodeCommand request, CancellationToken cancellationToken)
    {

        var operationToDelete = await _operationRepository.GetByCode(request.Code);
        if(operationToDelete == null) 
            return (Result)await Result.FailAsync();

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
