using BlazorApp.Application.Features.DocumentTypes;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BlazorApp.Application.Validators.Features.DocumentTypes;

public class AddEditDocumentTypeCommandValidator : AbstractValidator<AddEditDocumentTypeCommand>
{
    public AddEditDocumentTypeCommandValidator(IStringLocalizer<AddEditDocumentTypeCommandValidator> localizer)
    {
        RuleFor(request => request.Name)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
        RuleFor(request => request.Description)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Description is required!"]);
    }
}