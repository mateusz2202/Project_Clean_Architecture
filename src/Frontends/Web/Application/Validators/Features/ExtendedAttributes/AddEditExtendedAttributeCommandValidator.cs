using System;
using BlazorApp.Application.Features.ExtendedAttributes;
using BlazorApp.Application.Enums;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BlazorApp.Application.Validators.Features.ExtendedAttributes;

public class AddEditExtendedAttributeCommandValidatorLocalization
{
    // for localization
}

public abstract class AddEditExtendedAttributeCommandValidator<TId, TEntityId, TEntity, TExtendedAttribute> 
    : AbstractValidator<AddEditExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute>>
  
{
    protected AddEditExtendedAttributeCommandValidator(IStringLocalizer<AddEditExtendedAttributeCommandValidatorLocalization> localizer)
    {
        RuleFor(request => request.EntityId)
            .NotEqual(default(TEntityId)).WithMessage(x => localizer["Entity is required!"]);
        RuleFor(request => request.Key)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Key is required!"]);

        When(request => request.Type == EntityExtendedAttributeType.Decimal, () =>
        {
            RuleFor(request => request.Decimal).NotNull().WithMessage(x => string.Format(localizer["Decimal value is required using {0} type!"], x.Type.ToString()));
        });

        When(request => request.Type == EntityExtendedAttributeType.Text, () =>
        {
            RuleFor(request => request.Text).NotNull().WithMessage(x => string.Format(localizer["Text value is required using {0} type!"], x.Type.ToString()));
        });

        When(request => request.Type == EntityExtendedAttributeType.DateTime, () =>
        {
            RuleFor(request => request.DateTime).NotNull().WithMessage(x => string.Format(localizer["DateTime value is required using {0} type!"], x.Type.ToString()));
        });

        When(request => request.Type == EntityExtendedAttributeType.Json, () =>
        {
            RuleFor(request => request.Json).NotNull().WithMessage(x => string.Format(localizer["Json value is required using {0} type!"], x.Type.ToString()));
        });
    }
}