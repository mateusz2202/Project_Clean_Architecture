using FluentValidation;

namespace API_Identity.Models.Validator;

public class UpdatePasswordDTOValidator : AbstractValidator<UpdatePasswordDTO>
{
    public UpdatePasswordDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.OldPassword).MinimumLength(8);
        RuleFor(x => x.Password).MinimumLength(8);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
    }
}

