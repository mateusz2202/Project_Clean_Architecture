using BlazorApp.Application.Features.Products;
using BlazorApp.Application.Requests.Catalog;
using BlazorApp.Shared.Wrapper;
using System.Threading.Tasks;

namespace BlazorApp.Client.Infrastructure.Managers.Catalog.Product;

public interface IProductManager : IManager
{
    Task<PaginatedResult<GetAllPagedProductsResponse>> GetProductsAsync(GetAllPagedProductsRequest request);

    Task<IResult<string>> GetProductImageAsync(int id);

    Task<IResult<int>> SaveAsync(AddEditProductCommand request);

    Task<IResult<int>> DeleteAsync(int id);

    Task<IResult<string>> ExportToExcelAsync(string searchString = "");
}