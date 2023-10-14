using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Application.Features.Brands;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Brand;

public class BrandManager : IBrandManager
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BrandManager(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    private HttpClient ApiClient() => _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);

    public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        => await ApiClient().GetResult<string>(Routes.BrandsEndpoints.Export(searchString));

    public async Task<IResult<int>> DeleteAsync(int id)
        => await ApiClient().DeleteResult<int>($"{Routes.BrandsEndpoints.Delete}/{id}");

    public async Task<IResult<List<GetAllBrandsResponse>>> GetAllAsync()
        => await ApiClient().GetResult<List<GetAllBrandsResponse>>(Routes.BrandsEndpoints.GetAll);

    public async Task<IResult<int>> SaveAsync(AddEditBrandCommand request)
        => await ApiClient().PostAsJsonResult<int, AddEditBrandCommand>(Routes.BrandsEndpoints.Save, request);

    public async Task<IResult<int>> ImportAsync(ImportBrandsCommand request)
       => await ApiClient().PostAsJsonResult<int, ImportBrandsCommand>(Routes.BrandsEndpoints.Import, request);

}