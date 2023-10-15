using System;
using BlazorHero.CleanArchitecture.Domain.Contracts;

namespace BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes;

public record ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute>
(string SearchString = "", TEntityId EntityId = default, bool IncludeEntity = false, bool OnlyCurrentGroup = false, string CurrentGroup = "")    
        where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
        where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
        where TId : IEquatable<TId>;
