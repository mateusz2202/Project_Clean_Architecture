using MediatR;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Operation.Application.Common.Exceptions;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Contracts.Services;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Commands.AddAtributes;

public record AddOperationAttributCommad(object Attributes) : IRequest<Result>;

public class AddOperationAttributCommaddHandler : IRequestHandler<AddOperationAttributCommad, Result>
{
    private readonly IOperationService _operationService;
    private readonly IOperationRepository _operationRepository;
    public AddOperationAttributCommaddHandler(
        IOperationRepository operationRepository,
        IOperationService operationService)
    {
        _operationRepository = operationRepository;
        _operationService = operationService;
    }

    public async Task<Result> Handle(AddOperationAttributCommad request, CancellationToken cancellationToken)
    {

        var item = JsonConvert.DeserializeObject<dynamic>(request.Attributes.ToString());

        var idOperation = (int?)((dynamic)item).id ?? throw new NotFoundException("not found operation");
        
        var codeOperation = (string?)((dynamic)item).code;
        
        if (string.IsNullOrEmpty(codeOperation))
            throw new NotFoundException("not found operation");

        var existOperation = await _operationRepository.Any(x => x.Id == idOperation && x.Code == codeOperation);
        if (!existOperation)
            throw new NotFoundException("not found operation");

        await _operationService.AddOrEditAtributes(idOperation.ToString(), new PartitionKey(codeOperation), item, cancellationToken);

        _operationService.SendInfoAddedOperation();

        return (Result)await Result.SuccessAsync();
    }

}
