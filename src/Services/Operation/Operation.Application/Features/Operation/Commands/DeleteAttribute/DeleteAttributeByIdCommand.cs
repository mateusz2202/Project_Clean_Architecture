using MediatR;
using Microsoft.Azure.Cosmos;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Contracts.Services;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Commands.DeleteAttribute;

public record DeleteAttributeByIdCommand : IRequest<Result>
{
    public int Id { get; set; }
}

public class DeleteAttributeByIdCommandHandler : IRequestHandler<DeleteAttributeByIdCommand, Result>
{
    private readonly IOperationService _operationService;
    private readonly IUnitOfWork<int> _unitOfWork;
    public DeleteAttributeByIdCommandHandler(    
        IOperationService operationService,
        IUnitOfWork<int> unitOfWork)
    {    
        _operationService = operationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteAttributeByIdCommand request, CancellationToken cancellationToken)
    {

        var operationToDelete = await _unitOfWork.Repository<Domain.Entities.Operation>().GetByIdAsync(request.Id);
        if (operationToDelete == null)
            return (Result)await Result.FailAsync();

        await _operationService.DeleteAttribute(operationToDelete.Id.ToString(), new PartitionKey(operationToDelete.Code), cancellationToken);

        return (Result)await Result.SuccessAsync();
    }

}
