using Document.Application.Extensions;
using Document.Application.Interfaces.Repositories;
using Document.Application.Interfaces.Services;
using Document.Application.Specifications;
using Document.Domain.Entities;
using Document.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Document.Application.Features.DocumentTypes.Queries.Export;

public class ExportDocumentTypesQuery : IRequest<Result<string>>
{
    public string SearchString { get; set; }

    public ExportDocumentTypesQuery(string searchString = "")
    {
        SearchString = searchString;
    }
}

internal class ExportDocumentTypesQueryHandler : IRequestHandler<ExportDocumentTypesQuery, Result<string>>
{
    private readonly IExcelService _excelService;
    private readonly IUnitOfWork<int> _unitOfWork; 

    public ExportDocumentTypesQueryHandler(IExcelService excelService, IUnitOfWork<int> unitOfWork)
    {
        _excelService = excelService;
        _unitOfWork = unitOfWork;    
    }

    public async Task<Result<string>> Handle(ExportDocumentTypesQuery request, CancellationToken cancellationToken)
    {
        var documentTypeFilterSpec = new DocumentTypeFilterSpecification(request.SearchString);
        var documentTypes = await _unitOfWork.Repository<DocumentType>().Entities
            .Specify(documentTypeFilterSpec)
            .ToListAsync(cancellationToken);
        var data = await _excelService.ExportAsync(documentTypes, mappers: new Dictionary<string, Func<DocumentType, object>>
            {
                { "Id", item => item.Id },
                { "Name", item => item.Name },
                { "Description", item => item.Description }
            }, sheetName: "Document Types");

        return await Result<string>.SuccessAsync(data: data);
    }
}
