﻿using BlazorApp.Application.Features.Products;
using BlazorApp.Application.Requests.Catalog;
using BlazorApp.Client.Infrastructure.Extensions;
using BlazorApp.Shared.Constants.Application;
using BlazorApp.Shared.Wrapper;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp.Client.Infrastructure.Managers.Catalog.Product;

public class ProductManager : IProductManager
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductManager(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient ApiClient() => _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.ApiGateway);

    public async Task<IResult<int>> DeleteAsync(int id)
        => await ApiClient().DeleteResult<int>($"{Routes.ProductsEndpoints.Delete}/{id}");


    public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
       => await ApiClient().GetResult<string>(Routes.ProductsEndpoints.Export(searchString));


    public async Task<IResult<string>> GetProductImageAsync(int id)
        => await ApiClient().GetResult<string>(Routes.ProductsEndpoints.GetProductImage(id));


    public async Task<PaginatedResult<GetAllPagedProductsResponse>> GetProductsAsync(GetAllPagedProductsRequest request)
        => await ApiClient().GetPaginatedResult<GetAllPagedProductsResponse>(Routes.ProductsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));


    public async Task<IResult<int>> SaveAsync(AddEditProductCommand request)
        => await ApiClient().PostAsJsonResult<int, AddEditProductCommand>(Routes.ProductsEndpoints.Save, request);


}