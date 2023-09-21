using FluentValidation;
using OperationAPI.Data;

namespace OperationAPI.Models.Validators;

public class CreateOperationWithAttributeDTOValidator : AbstractValidator<CreateOperationWithAttributeDTO>
{
    private readonly OperationDbContext _dbContext;
    public CreateOperationWithAttributeDTOValidator(OperationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.CreateOperationDTO)
            .SetValidator(new CreateOperationDTOValidator(dbContext))
            .WithMessage(x => $"Incorrect CreateOperationDTO");
    }

}
