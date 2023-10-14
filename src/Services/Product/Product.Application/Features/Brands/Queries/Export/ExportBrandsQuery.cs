using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Application.Extensions;
using Product.Application.Interfaces.Repositories;
using Product.Application.Interfaces.Services;
using Product.Application.Specifications;
using Product.Domain.Entities;
using Product.Shared.Wrapper;

namespace Product.Application.Application.Features.Brands.Queries.Export;

public record ExportBrandsQuery(string SearchString = "") : IRequest<Result<string>>;

internal class ExportBrandsQueryHandler : IRequestHandler<ExportBrandsQuery, Result<string>>
{
    private readonly IExcelService _excelService;
    private readonly IUnitOfWork<int> _unitOfWork;

    public ExportBrandsQueryHandler(IExcelService excelService, IUnitOfWork<int> unitOfWork)
    {
        _excelService = excelService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> Handle(ExportBrandsQuery request, CancellationToken cancellationToken)
    {
        var brandFilterSpec = new BrandFilterSpecification(request.SearchString);
        var brands = await _unitOfWork.Repository<Brand>().Entities
            .Specify(brandFilterSpec)
            .ToListAsync(cancellationToken);
        var data = await _excelService.ExportAsync(brands, mappers: new Dictionary<string, Func<Brand, object>>
        {
            { "Id", item => item.Id },
            { "Name", item => item.Name },
            { "Description", item => item.Description },
            { "Tax", item => item.Tax }
        }, sheetName: "Brands");

        return await Result<string>.SuccessAsync(data: data);
    }
}
