using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.DocumentTypes.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.DocumentTypes.Queries.GetAll;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Misc.DocumentType;

public class DocumentTypeManager : IDocumentTypeManager
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DocumentTypeManager(IHttpClientFactory httpClientFactory)
    {           
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
            ? Routes.DocumentTypesEndpoints.Export
            : Routes.DocumentTypesEndpoints.ExportFiltered(searchString));
        return await response.ToResult<string>();
    }

    public async Task<IResult<int>> DeleteAsync(int id)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.DeleteAsync($"{Routes.DocumentTypesEndpoints.Delete}/{id}");
        return await response.ToResult<int>();
    }

    public async Task<IResult<List<GetAllDocumentTypesResponse>>> GetAllAsync()
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.GetAsync(Routes.DocumentTypesEndpoints.GetAll);
        return await response.ToResult<List<GetAllDocumentTypesResponse>>();
    }

    public async Task<IResult<int>> SaveAsync(AddEditDocumentTypeCommand request)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.PostAsJsonAsync(Routes.DocumentTypesEndpoints.Save, request);
        return await response.ToResult<int>();
    }
}