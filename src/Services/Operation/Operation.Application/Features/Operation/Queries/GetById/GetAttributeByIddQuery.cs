using MediatR;
using Microsoft.Azure.Cosmos;
using Operation.Application.Common.Exceptions;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Contracts.Services;
using Operation.Shared.Wrapper;
using System.Dynamic;

namespace Operation.Application.Features.Operation.Queries.GetById;


public class GetAttributeByIddQuery : IRequest<Result<ExpandoObject>>
{
    public int Id { get; set; }
}

internal class GetAttributeByIdHandler : IRequestHandler<GetAttributeByIddQuery, Result<ExpandoObject>>
{
    private readonly IUnitOfWork<int> _unitOfWork;  
    private readonly IOperationService _operationService;

    public GetAttributeByIdHandler(IUnitOfWork<int> unitOfWork, IOperationService operationService)
    {
        _unitOfWork = unitOfWork;     
        _operationService = operationService;
    }

    public async Task<Result<ExpandoObject>> Handle(GetAttributeByIddQuery query, CancellationToken cancellationToken)
    {
        var operation = await _unitOfWork.Repository<Domain.Entities.Operation>().GetByIdAsync(query.Id) ?? throw new NotFoundException("Not found operation"); ;
        var attribute = await _operationService.GetAttribute(operation.Id.ToString(), new PartitionKey(operation.Code));
        return await Result<ExpandoObject>.SuccessAsync(attribute);
    }
}
