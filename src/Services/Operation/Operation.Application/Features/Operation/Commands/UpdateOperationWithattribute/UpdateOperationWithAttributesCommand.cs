using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Contracts.Services;
using Operation.Application.Features.Operation.Commands.AddOperation;
using Operation.Application.Features.Operation.Queries.GetById;
using Operation.Shared.Constans;
using Operation.Shared.Wrapper;


namespace Operation.Application.Features.Operation.Commands.UpdateOperationWithattribute;

public record UpdateOperationWithAttributesCommand : IRequest<Result>
{
    public int Id { get; set; }
    public AddOperationCommand AddOperationCommand { get; set; } = null!;
    public object Attributes { get; set; } = null!;
}

public class UpdateOperationWithAttributesCommandHandler : IRequestHandler<UpdateOperationWithAttributesCommand, Result>
{
    private readonly IOperationService _operationService;
    private readonly IMediator _mediator;
    private readonly IUnitOfWork<int> _unitOfWork;
    public UpdateOperationWithAttributesCommandHandler(   
        IOperationService operationService,
        IMediator mediator,
        IUnitOfWork<int> unitOfWork)
    {      
        _operationService = operationService;
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateOperationWithAttributesCommand request, CancellationToken cancellationToken)
    {

        var operationToUpdate = (await _mediator.Send(new GetOperationByIdQuery() { Id = request.Id }, cancellationToken)).Data;

        string oldCode = operationToUpdate.Code;
        operationToUpdate.Code = request.AddOperationCommand.Code;
        operationToUpdate.Name = request.AddOperationCommand.Name;
        await _unitOfWork.CommitAndRemoveCache(cancellationToken,
                                   new string[]
                                   {    ApplicationConstants.Cache.OPERATION_KEY ,
                                        ApplicationConstants.Cache.OPERATIONATTRIBUTE_KEY,
                                        ApplicationConstants.Cache.OPERATIONWITHATTRIBUTE_KEY,
                                   });

        var item = JsonConvert.DeserializeObject<dynamic>(request.Attributes.ToString());
        ((dynamic)item).id = operationToUpdate.Id.ToString();
        ((dynamic)item).code = operationToUpdate.Code;

        await _operationService.AddOrEditAtributes(operationToUpdate.Id.ToString(), new PartitionKey(oldCode), item, cancellationToken);
        
        return await Result<int>.SuccessAsync();
    }

}