using BlazorHero.CleanArchitecture.Application.Features.Documents.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Documents.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Requests.Documents;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Documents.Queries.GetById;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Misc.Document;

public class DocumentManager : IDocumentManager
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DocumentManager(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IResult<int>> DeleteAsync(int id)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.DeleteAsync($"{Routes.DocumentsEndpoints.Delete}/{id}");
        return await response.ToResult<int>();
    }

    public async Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.GetAsync(Routes.DocumentsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString));
        return await response.ToPaginatedResult<GetAllDocumentsResponse>();
    }

    public async Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(GetDocumentByIdQuery request)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.GetAsync(Routes.DocumentsEndpoints.GetById(request.Id));
        return await response.ToResult<GetDocumentByIdResponse>();
    }

    public async Task<IResult<int>> SaveAsync(AddEditDocumentCommand request)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.PostAsJsonAsync(Routes.DocumentsEndpoints.Save, request);
        return await response.ToResult<int>();
    }
}