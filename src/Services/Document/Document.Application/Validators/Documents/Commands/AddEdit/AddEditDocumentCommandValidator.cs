using Document.Application.Features.Documents.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Document.Application.Validators.Documents.Commands.AddEdit;

public class AddEditDocumentCommandValidator : AbstractValidator<AddEditDocumentCommand>
{
    public AddEditDocumentCommandValidator()
    {
        RuleFor(request => request.Title)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Title is required!");
        RuleFor(request => request.Description)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Description is required!");
        RuleFor(request => request.DocumentTypeId)
            .GreaterThan(0).WithMessage(x => "Document Type is required!");
        RuleFor(request => request.URL)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "File is required!");
    }
}
