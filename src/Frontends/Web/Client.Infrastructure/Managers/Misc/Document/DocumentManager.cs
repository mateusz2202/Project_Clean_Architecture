using BlazorHero.CleanArchitecture.Application.Requests.Documents;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Application.Features.Documents;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Misc.Document;

public class DocumentManager : IDocumentManager
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DocumentManager(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    private HttpClient ApiClient() => _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);

    public async Task<IResult<int>> DeleteAsync(int id)
        => await ApiClient().DeleteResult<int>($"{Routes.DocumentsEndpoints.Delete}/{id}");

    public async Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request)
        => await ApiClient().GetPaginatedResult<GetAllDocumentsResponse>(Routes.DocumentsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString));

    public async Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(int id)
        => await ApiClient().GetResult<GetDocumentByIdResponse>(Routes.DocumentsEndpoints.GetById(id));

    public async Task<IResult<int>> SaveAsync(AddEditDocumentCommand request)
        => await ApiClient().PostAsJsonResult<int, AddEditDocumentCommand>(Routes.DocumentsEndpoints.Save, request);


}