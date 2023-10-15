using FluentValidation;
using Product.Application.Application.Features.Brands.Commands.AddEdit;

namespace Product.Application.Validators.Brands.Commands.AddEdit;

public class AddEditBrandCommandValidator : AbstractValidator<AddEditBrandCommand>
{
    public AddEditBrandCommandValidator()
    {
        RuleFor(request => request.Name)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Name is required!");
        RuleFor(request => request.Description)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Description is required!");
        RuleFor(request => request.Tax)
            .GreaterThan(0).WithMessage(x => "Tax must be greater than 0");
    }
}
