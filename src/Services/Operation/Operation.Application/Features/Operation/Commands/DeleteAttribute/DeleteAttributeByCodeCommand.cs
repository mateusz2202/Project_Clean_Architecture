using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Localization;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Contracts.Services;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Commands.DeleteAttribute;


public record DeleteAttributeByCodeCommand : IRequest<Result>
{
    public string Code { get; set; } = string.Empty;
}

public class DeleteAttributeByCodeCommandHandler : IRequestHandler<DeleteAttributeByCodeCommand, Result>
{
    private readonly IOperationService _operationService;
    private readonly IOperationRepository _operationRepository;
    public DeleteAttributeByCodeCommandHandler(   
        IOperationService operationService,   
        IOperationRepository operationRepository)
    {       
        _operationService = operationService;     
        _operationRepository = operationRepository;
    }

    public async Task<Result> Handle(DeleteAttributeByCodeCommand request, CancellationToken cancellationToken)
    {

        var operationToDelete = await _operationRepository.GetByCode(request.Code);
        if (operationToDelete == null)
            return (Result)await Result.FailAsync();

        await _operationService.DeleteAttribute(operationToDelete.Id.ToString(), new PartitionKey(operationToDelete.Code), cancellationToken);
        
        return (Result)await Result.SuccessAsync();
    }

}
