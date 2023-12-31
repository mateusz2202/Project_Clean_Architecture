﻿using BlazorApp.Application.Features.Documents;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BlazorApp.Application.Validators.Features.Documents;

public class AddEditDocumentCommandValidator : AbstractValidator<AddEditDocumentCommand>
{
    public AddEditDocumentCommandValidator(IStringLocalizer<AddEditDocumentCommandValidator> localizer)
    {
        RuleFor(request => request.Title)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Title is required!"]);
        RuleFor(request => request.Description)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Description is required!"]);
        RuleFor(request => request.DocumentTypeId)
            .GreaterThan(0).WithMessage(x => localizer["Document Type is required!"]);
        RuleFor(request => request.URL)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["File is required!"]);
    }
}