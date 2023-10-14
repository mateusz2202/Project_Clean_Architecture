using Document.Application.Extensions;
using Document.Application.Interfaces.Repositories;
using Document.Application.Interfaces.Services;
using Document.Application.Specifications;
using Document.Domain.Contracts;
using Document.Domain.Enums;
using Document.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Globalization;


namespace Document.Application.Features.ExtendedAttributes.Queries.Export;

internal class ExportExtendedAttributesQueryLocalization
{
    // for localization
}

public class ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute>
    : IRequest<Result<string>>
        where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
        where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
        where TId : IEquatable<TId>
{
    public string SearchString { get; set; }
    public TEntityId EntityId { get; set; }
    public bool IncludeEntity { get; set; }
    public bool OnlyCurrentGroup { get; set; }
    public string CurrentGroup { get; set; }

    public ExportExtendedAttributesQuery(string searchString = "", TEntityId entityId = default, bool includeEntity = false, bool onlyCurrentGroup = false, string currentGroup = "")
    {
        SearchString = searchString;
        EntityId = entityId;
        IncludeEntity = includeEntity;
        OnlyCurrentGroup = onlyCurrentGroup;
        CurrentGroup = currentGroup;
    }
}

internal class ExportExtendedAttributesQueryHandler<TId, TEntityId, TEntity, TExtendedAttribute>
    : IRequestHandler<ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute>, Result<string>>
        where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
        where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
        where TId : IEquatable<TId>
{
    private readonly IExcelService _excelService;
    private readonly IUnitOfWork<TId> _unitOfWork;  

    public ExportExtendedAttributesQueryHandler(IExcelService excelService
        , IUnitOfWork<TId> unitOfWork
        , IStringLocalizer<ExportExtendedAttributesQueryLocalization> localizer)
    {
        _excelService = excelService;
        _unitOfWork = unitOfWork;      
    }

    public async Task<Result<string>> Handle(ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute> request, CancellationToken cancellationToken)
    {
        var extendedAttributeFilterSpec = new ExtendedAttributeFilterSpecification<TId, TEntityId, TEntity, TExtendedAttribute>(request);
        var extendedAttributes = await _unitOfWork.Repository<TExtendedAttribute>().Entities
            .Specify(extendedAttributeFilterSpec)
            .ToListAsync(cancellationToken);

        // check SearchString outside of specification because of
        // an expression tree lambda may not contain a null propagating operator
        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            extendedAttributes = extendedAttributes.Where(p =>
                    p.Key.Contains(request.SearchString, StringComparison.InvariantCultureIgnoreCase)
                    || p.Decimal?.ToString().Contains(request.SearchString, StringComparison.InvariantCultureIgnoreCase) == true
                    || p.Text?.Contains(request.SearchString, StringComparison.InvariantCultureIgnoreCase) == true
                    || p.DateTime?.ToString("G", CultureInfo.CurrentCulture).Contains(request.SearchString, StringComparison.InvariantCultureIgnoreCase) == true
                    || p.Json?.Contains(request.SearchString, StringComparison.InvariantCultureIgnoreCase) == true
                    || p.ExternalId?.Contains(request.SearchString, StringComparison.InvariantCultureIgnoreCase) == true
                    || p.Description?.Contains(request.SearchString, StringComparison.InvariantCultureIgnoreCase) == true
                    || p.Group?.Contains(request.SearchString, StringComparison.InvariantCultureIgnoreCase) == true)
                .ToList();
        }

        var mappers = new Dictionary<string, Func<TExtendedAttribute, object>>
            {
                {"Id", item => item.Id},
                {"EntityId", item => item.EntityId},
                {"Type", item => item.Type},
                {"Key", item => item.Key},
                {
                    "Value", item => item.Type switch
                    {
                        EntityExtendedAttributeType.Decimal => item.Decimal,
                        EntityExtendedAttributeType.Text => item.Text,
                        EntityExtendedAttributeType.DateTime => item.DateTime != null ? DateTime.SpecifyKind((DateTime)item.DateTime, DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture) : string.Empty,
                        EntityExtendedAttributeType.Json => item.Json,
                        _ => throw new ArgumentOutOfRangeException(nameof(item.Type), "Type should be valid")
                    }
                },
                {"ExternalId", item => item.ExternalId},
                {"Group", item => item.Group},
                {"Description", item => item.Description},
                {"IsActive", item => item.IsActive}
            };

        if (request.IncludeEntity)
        {
            mappers.Add("EntityCreatedBy", item => item.Entity.CreatedBy);
            mappers.Add("EntityCreatedOn (Local)", item => item.Entity.CreatedOn.ToString("G", CultureInfo.CurrentCulture));
            mappers.Add("EntityCreatedOn (UTC)", item => DateTime.SpecifyKind(item.Entity.CreatedOn, DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture));
            mappers.Add("EntityLastModifiedBy", item => item.Entity.ModifiedBy);
            mappers.Add("EntityLastModifiedOn (Local)", item => item.Entity.ModifiedOn?.ToString("G", CultureInfo.CurrentCulture));
            mappers.Add("EntityLastModifiedOn (UTC)", item => item.Entity.ModifiedOn != null ? DateTime.SpecifyKind((DateTime)item.Entity.ModifiedOn, DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture) : string.Empty);
        }

        var data = await _excelService.ExportAsync(extendedAttributes, mappers: mappers,
            sheetName: string.Format("{0} Extended Attributes", typeof(TEntity).Name));

        return await Result<string>.SuccessAsync(data: data);
    }
}
