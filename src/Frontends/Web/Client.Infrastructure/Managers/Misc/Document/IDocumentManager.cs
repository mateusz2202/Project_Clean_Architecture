using BlazorApp.Application.Requests.Documents;
using BlazorApp.Shared.Wrapper;
using System.Threading.Tasks;
using BlazorApp.Application.Features.Documents;

namespace BlazorApp.Client.Infrastructure.Managers.Misc.Document;

public interface IDocumentManager : IManager
{
    Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request);

    Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(int id);

    Task<IResult<int>> SaveAsync(AddEditDocumentCommand request);

    Task<IResult<int>> DeleteAsync(int id);
}