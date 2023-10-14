using BlazorHero.CleanArchitecture.Application.Features.Products.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Products.Queries.GetAllPaged;
using BlazorHero.CleanArchitecture.Application.Requests.Catalog;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Product;

public class ProductManager : IProductManager
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductManager(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IResult<int>> DeleteAsync(int id)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.DeleteAsync($"{Routes.ProductsEndpoints.Delete}/{id}");
        return await response.ToResult<int>();
    }

    public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
            ? Routes.ProductsEndpoints.Export
            : Routes.ProductsEndpoints.ExportFiltered(searchString));
        return await response.ToResult<string>();
    }

    public async Task<IResult<string>> GetProductImageAsync(int id)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.GetAsync(Routes.ProductsEndpoints.GetProductImage(id));
        return await response.ToResult<string>();
    }

    public async Task<PaginatedResult<GetAllPagedProductsResponse>> GetProductsAsync(GetAllPagedProductsRequest request)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.GetAsync(Routes.ProductsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
        return await response.ToPaginatedResult<GetAllPagedProductsResponse>();
    }

    public async Task<IResult<int>> SaveAsync(AddEditProductCommand request)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);
        var response = await httpClient.PostAsJsonAsync(Routes.ProductsEndpoints.Save, request);
        return await response.ToResult<int>();
    }
}