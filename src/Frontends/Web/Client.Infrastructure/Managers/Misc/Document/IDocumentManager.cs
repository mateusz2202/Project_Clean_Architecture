using BlazorHero.CleanArchitecture.Application.Requests.Documents;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Documents;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Misc.Document;

public interface IDocumentManager : IManager
{
    Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request);

    Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(int id);

    Task<IResult<int>> SaveAsync(AddEditDocumentCommand request);

    Task<IResult<int>> DeleteAsync(int id);
}