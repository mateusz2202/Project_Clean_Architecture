using Document.Application.Extensions;
using Document.Application.Interfaces.Repositories;
using Document.Application.Interfaces.Services;
using Document.Application.Specifications;
using Document.Shared.Wrapper;
using MediatR;
using System.Linq.Expressions;

namespace Document.Application.Features.Documents.Queries.GetAll;

public record GetAllDocumentsQuery(int PageNumber, int PageSize, string SearchString) : IRequest<PaginatedResult<GetAllDocumentsResponse>>;

internal class GetAllDocumentsQueryHandler : IRequestHandler<GetAllDocumentsQuery, PaginatedResult<GetAllDocumentsResponse>>
{
    private readonly IUnitOfWork<int> _unitOfWork;

    private readonly ICurrentUserService _currentUserService;

    public GetAllDocumentsQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedResult<GetAllDocumentsResponse>> Handle(GetAllDocumentsQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Domain.Entities.Document, GetAllDocumentsResponse>> expression = e => new GetAllDocumentsResponse
        {
            Id = e.Id,
            Title = e.Title,
            CreatedBy = e.CreatedBy,
            IsPublic = e.IsPublic,
            CreatedOn = e.CreatedOn,
            Description = e.Description,
            URL = e.URL,
            DocumentType = e.DocumentType.Name,
            DocumentTypeId = e.DocumentTypeId
        };
        var docSpec = new DocumentFilterSpecification(request.SearchString, _currentUserService.UserId);
        var data = await _unitOfWork.Repository<Domain.Entities.Document>().Entities
           .Specify(docSpec)
           .Select(expression)
           .ToPaginatedListAsync(request.PageNumber, request.PageSize);
        return data;
    }
}