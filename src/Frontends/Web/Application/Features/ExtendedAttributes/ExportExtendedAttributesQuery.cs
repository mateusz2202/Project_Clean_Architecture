using System;
using BlazorHero.CleanArchitecture.Domain.Contracts;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;

namespace BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes;

internal class ExportExtendedAttributesQueryLocalization
{
    // for localization
}

public record ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute>
    (string SearchString = "", TEntityId EntityId = default, bool IncludeEntity = false, bool OnlyCurrentGroup = false, string CurrentGroup = "")
    : IRequest<Result<string>>
        where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
        where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
        where TId : IEquatable<TId>;
