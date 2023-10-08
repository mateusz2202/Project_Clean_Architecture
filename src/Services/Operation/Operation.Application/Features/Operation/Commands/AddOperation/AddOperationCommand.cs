using MediatR;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Commands.AddOperation;

public class AddOperationCommand : IRequest<Result<int>>
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;
}
