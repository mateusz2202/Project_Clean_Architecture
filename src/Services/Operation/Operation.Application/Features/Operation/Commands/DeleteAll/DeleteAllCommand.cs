using MediatR;
using Microsoft.Extensions.Localization;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Contracts.Services;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Commands.DeleteAll;

public record DeleteAllCommand : IRequest<Result> { }

public class DeleteAllCommandHandler : IRequestHandler<DeleteAllCommand, Result>
{    
    private readonly IOperationService _operationService;
    private readonly IUnitOfWork<int> _unitOfWork;
    public DeleteAllCommandHandler(     
        IOperationService operationService,
        IUnitOfWork<int> unitOfWork)
    {    
        _operationService = operationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteAllCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.Repository<Domain.Entities.Operation>().DeleteAsync(x => true);
        await _operationService.DeleteAllAtribue();
        return (Result)await Result.SuccessAsync();
    }

}
