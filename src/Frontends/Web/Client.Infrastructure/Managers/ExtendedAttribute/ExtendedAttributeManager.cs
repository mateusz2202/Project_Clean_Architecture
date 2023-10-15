using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorApp.Application.Features.ExtendedAttributes;
using BlazorApp.Client.Infrastructure.Extensions;
using BlazorApp.Shared.Constants.Application;
using BlazorApp.Shared.Wrapper;

namespace BlazorApp.Client.Infrastructure.Managers.ExtendedAttribute;

public class ExtendedAttributeManager<TId, TEntityId, TEntity, TExtendedAttribute> 
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ExtendedAttributeManager(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient ApiClient() => _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);

    public async Task<IResult<string>> ExportToExcelAsync(ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute> request)
        => await ApiClient().GetResult<string>(string.IsNullOrWhiteSpace(request.SearchString) && !request.IncludeEntity && !request.OnlyCurrentGroup
            ? Routes.ExtendedAttributesEndpoints.Export(typeof(TEntity).Name, request.EntityId, request.IncludeEntity, request.OnlyCurrentGroup, request.CurrentGroup)
            : Routes.ExtendedAttributesEndpoints.ExportFiltered(typeof(TEntity).Name, request.SearchString, request.EntityId, request.IncludeEntity, request.OnlyCurrentGroup, request.CurrentGroup));


    public async Task<IResult<TId>> DeleteAsync(TId id)
        => await ApiClient().DeleteResult<TId>($"{Routes.ExtendedAttributesEndpoints.Delete(typeof(TEntity).Name)}/{id}");

    public async Task<IResult<List<GetAllExtendedAttributesResponse<TId, TEntityId>>>> GetAllAsync()
        => await ApiClient().GetResult<List<GetAllExtendedAttributesResponse<TId, TEntityId>>>(Routes.ExtendedAttributesEndpoints.GetAll(typeof(TEntity).Name));

    public async Task<IResult<List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>>>> GetAllByEntityIdAsync(TEntityId entityId)
        => await ApiClient().GetResult<List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>>>(Routes.ExtendedAttributesEndpoints.GetAllByEntityId(typeof(TEntity).Name, entityId));

    public async Task<IResult<TId>> SaveAsync(AddEditExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute> request)
        => await ApiClient().PostAsJsonResult<TId, AddEditExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute>>(Routes.ExtendedAttributesEndpoints.Save(typeof(TEntity).Name), request);


}