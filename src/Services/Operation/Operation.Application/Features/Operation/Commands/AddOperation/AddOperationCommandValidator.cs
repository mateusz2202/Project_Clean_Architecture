using FluentValidation;
using Operation.Application.Contracts.Repositories;

namespace Operation.Application.Features.Operation.Commands.AddOperation;

public class AddOperationCommandValidator : AbstractValidator<AddOperationCommand>
{
    private readonly IOperationRepository _operationRepository;
    public AddOperationCommandValidator(IOperationRepository operationRepository)
    {
        _operationRepository = operationRepository;
        RuleFor(x => x.Code)
            .Must(x => !UniqueOperationCode(x))
            .WithMessage(x => $"{x.Code} already exists");

    }

    private bool UniqueOperationCode(string code)
        => _operationRepository.IsCodeUsed(code).Result;
}
