using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.DocumentTypes;
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
    private HttpClient ApiClient() => _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);

    public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        => await ApiClient().GetResult<string>(Routes.DocumentTypesEndpoints.Export(searchString));

    public async Task<IResult<int>> DeleteAsync(int id)
        => await ApiClient().DeleteResult<int>($"{Routes.DocumentTypesEndpoints.Delete}/{id}");


    public async Task<IResult<List<GetAllDocumentTypesResponse>>> GetAllAsync()
        => await ApiClient().GetResult<List<GetAllDocumentTypesResponse>>(Routes.DocumentTypesEndpoints.GetAll);


    public async Task<IResult<int>> SaveAsync(AddEditDocumentTypeCommand request)
        => await ApiClient().PostAsJsonResult<int, AddEditDocumentTypeCommand>(Routes.DocumentTypesEndpoints.Save, request);


}