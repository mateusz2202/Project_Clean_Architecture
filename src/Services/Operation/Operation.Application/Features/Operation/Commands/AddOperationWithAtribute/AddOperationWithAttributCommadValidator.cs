using FluentValidation;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Features.Operation.Commands.AddOperation;

namespace Operation.Application.Features.Operation.Commands.AddOperationWithAtribute;

public class AddOperationWithAttributCommadValidator : AbstractValidator<AddOperationWithAttributCommad>
{
    private readonly IOperationRepository _operationRepository;
    public AddOperationWithAttributCommadValidator(IOperationRepository operationRepository)
    {
        _operationRepository = operationRepository;

        RuleFor(x => x.AddOperationCommand)
            .SetValidator(new AddOperationCommandValidator(_operationRepository))
            .WithMessage(x => $"Incorrect");
    }
}
