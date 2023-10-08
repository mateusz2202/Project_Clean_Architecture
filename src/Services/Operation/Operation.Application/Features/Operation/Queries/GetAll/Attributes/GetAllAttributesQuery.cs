using MediatR;
using Operation.Shared.Wrapper;
using System.Dynamic;

namespace Operation.Application.Features.Operation.Queries.GetAll.Attributes;

public class GetAllAttributesQuery : IRequest<Result<List<ExpandoObject>>>
{
    public GetAllAttributesQuery()
    {

    }
}
