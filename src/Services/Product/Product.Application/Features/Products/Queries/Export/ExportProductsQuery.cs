using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Shared.Wrapper;
using Product.Application.Interfaces.Repositories;
using Product.Application.Extensions;
using Product.Application.Specifications;
using Product.Application.Interfaces.Services;

namespace Product.Application.Application.Features.Products.Queries.Export;

public record ExportProductsQuery(string SearchString = "") : IRequest<Result<string>>;


internal class ExportProductsQueryHandler : IRequestHandler<ExportProductsQuery, Result<string>>
{
    private readonly IExcelService _excelService;
    private readonly IUnitOfWork<int> _unitOfWork;

    public ExportProductsQueryHandler(IExcelService excelService, IUnitOfWork<int> unitOfWork)
    {
        _excelService = excelService;
        _unitOfWork = unitOfWork;  
    }

    public async Task<Result<string>> Handle(ExportProductsQuery request, CancellationToken cancellationToken)
    {
        var productFilterSpec = new ProductFilterSpecification(request.SearchString);
        var products = await _unitOfWork.Repository<Domain.Entities.Product>().Entities
            .Specify(productFilterSpec)
            .ToListAsync(cancellationToken);
        var data = await _excelService.ExportAsync(products, mappers: new Dictionary<string, Func<Domain.Entities.Product, object>>
        {
            {"Id", item => item.Id },
            {"Name", item => item.Name },
            {"Barcode", item => item.Barcode },
            {"Description", item => item.Description },
            {"Rate", item => item.Rate }
        }, sheetName: "Products");

        return await Result<string>.SuccessAsync(data: data);
    }
}