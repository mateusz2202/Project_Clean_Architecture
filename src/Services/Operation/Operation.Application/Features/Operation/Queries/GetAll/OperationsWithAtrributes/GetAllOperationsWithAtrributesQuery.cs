using MediatR;
using Operation.Shared.Wrapper;
using System.Dynamic;

namespace Operation.Application.Features.Operation.Queries.GetAll.OperationsWithAtrributes;

public class GetAllOperationsWithAtrributesQuery : IRequest<Result<List<ExpandoObject>>>
{
    public GetAllOperationsWithAtrributesQuery()
    {
        
    }
}
