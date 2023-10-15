using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp.Application.Features.DocumentTypes;
using BlazorApp.Shared.Wrapper;

namespace BlazorApp.Client.Infrastructure.Managers.Misc.DocumentType
{
    public interface IDocumentTypeManager : IManager
    {
        Task<IResult<List<GetAllDocumentTypesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditDocumentTypeCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}