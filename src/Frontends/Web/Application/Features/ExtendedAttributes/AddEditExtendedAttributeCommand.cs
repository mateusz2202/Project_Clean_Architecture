﻿#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using BlazorHero.CleanArchitecture.Domain.Contracts;
using BlazorHero.CleanArchitecture.Domain.Enums;

namespace BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes;

internal class AddEditExtendedAttributeCommandLocalization
{
    // for localization
}

public class AddEditExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute>    
        where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
        where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
        where TId : IEquatable<TId>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public TId Id { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public TEntityId EntityId { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public EntityExtendedAttributeType Type { get; set; }

    [Required(AllowEmptyStrings = false)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string Key { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public string? Text { get; set; }

    public decimal? Decimal { get; set; }

    public DateTime? DateTime { get; set; }

    public string? Json { get; set; }

    public string? ExternalId { get; set; }

    public string? Group { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }
}