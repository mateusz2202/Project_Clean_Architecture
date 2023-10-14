using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Brands;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Brand;

public interface IBrandManager : IManager
{
    Task<IResult<List<GetAllBrandsResponse>>> GetAllAsync();

    Task<IResult<int>> SaveAsync(AddEditBrandCommand request);

    Task<IResult<int>> DeleteAsync(int id);

    Task<IResult<string>> ExportToExcelAsync(string searchString = "");

    Task<IResult<int>> ImportAsync(ImportBrandsCommand request);
}