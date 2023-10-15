using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp.Application.Features.ExtendedAttributes;
using BlazorApp.Shared.Wrapper;

namespace BlazorApp.Client.Infrastructure.Managers.ExtendedAttribute;

public interface IExtendedAttributeManager<TId, TEntityId, TEntity, TExtendedAttribute>
{
    Task<IResult<List<GetAllExtendedAttributesResponse<TId, TEntityId>>>> GetAllAsync();

    Task<IResult<List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>>>> GetAllByEntityIdAsync(TEntityId entityId);

    Task<IResult<TId>> SaveAsync(AddEditExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute> request);

    Task<IResult<TId>> DeleteAsync(TId id);

    Task<IResult<string>> ExportToExcelAsync(ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute> request);
}