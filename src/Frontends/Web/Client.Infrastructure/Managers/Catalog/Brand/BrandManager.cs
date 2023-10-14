using BlazorHero.CleanArchitecture.Application.Features.Brands.Queries.GetAll;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.Import;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Brand;

public class BrandManager : IBrandManager
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BrandManager(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
            ? Routes.BrandsEndpoints.Export
            : Routes.BrandsEndpoints.ExportFiltered(searchString));
        return await response.ToResult<string>();
    }

    public async Task<IResult<int>> DeleteAsync(int id)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.DeleteAsync($"{Routes.BrandsEndpoints.Delete}/{id}");
        return await response.ToResult<int>();
    }

    public async Task<IResult<List<GetAllBrandsResponse>>> GetAllAsync()
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.GetAsync(Routes.BrandsEndpoints.GetAll);
        return await response.ToResult<List<GetAllBrandsResponse>>();
    }

    public async Task<IResult<int>> SaveAsync(AddEditBrandCommand request)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.PostAsJsonAsync(Routes.BrandsEndpoints.Save, request);
        return await response.ToResult<int>();
    }

    public async Task<IResult<int>> ImportAsync(ImportBrandsCommand request)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.PostAsJsonAsync(Routes.BrandsEndpoints.Import, request);
        return await response.ToResult<int>();
    }
}