using FluentValidation;
using OperationAPI.Data;

namespace OperationAPI.Models.Validators;

public class CreateOperationDTOValidator : AbstractValidator<CreateOperationDTO>
{
    private readonly OperationDbContext _dbContext;
    public CreateOperationDTOValidator(OperationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Code)
            .Must(x => !UniqueOperationCode(x))
            .WithMessage(x => $"{x.Code} already exists");
    }

    private bool UniqueOperationCode(string code) => _dbContext.Operations.Any(x => x.Code.ToLower() == code.ToLower());
}
