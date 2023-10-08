using MediatR;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Queries.GetAll.Operations;

public class GetAllOperationsQuery : IRequest<Result<List<GetAllOperationsResponse>>>
{
    public GetAllOperationsQuery()
    {
    }
}