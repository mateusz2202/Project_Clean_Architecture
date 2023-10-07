using API_Identity.Data;
using FluentValidation;

namespace API_Identity.Models.Validator;

public class RegisterUserDTOValidator : AbstractValidator<RegisterUserDTO>
{
    public RegisterUserDTOValidator(IndentityDbContext indentityDbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2);
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Custom((v, y) =>
            {
                var loginUse = indentityDbContext.Users?.Any(x => x.Email.ToLower() == v.ToLower());
                if (loginUse != null && loginUse == true)
                    y.AddFailure("Email", "Adres email jest zajęty");
            });
        RuleFor(x => x.Password).MinimumLength(8);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);

    }
}
