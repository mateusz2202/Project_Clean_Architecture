using FluentValidation;
using Product.Application.Application.Features.Products.Commands.AddEdit;

namespace Product.Application.Validators.Products.Commands.AddEdit;

public class AddEditProductCommandValidator : AbstractValidator<AddEditProductCommand>
{
    public AddEditProductCommandValidator()
    {
        RuleFor(request => request.Name)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Name is required!");
        RuleFor(request => request.Barcode)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Barcode is required!");
        RuleFor(request => request.Description)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Description is required!");
        RuleFor(request => request.BrandId)
            .GreaterThan(0).WithMessage(x => "Brand is required!");
        RuleFor(request => request.Rate)
            .GreaterThan(0).WithMessage(x => "Rate must be greater than 0");
    }
}
