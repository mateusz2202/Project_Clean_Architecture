using Document.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Document.Persistence.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private readonly IRepositoryAsync<Domain.Entities.Document, int> _repository;

    public DocumentRepository(IRepositoryAsync<Domain.Entities.Document, int> repository)
    {
        _repository = repository;
    }

    public async Task<bool> IsDocumentTypeUsed(int documentTypeId)
    {
        return await _repository.Entities.AnyAsync(b => b.DocumentTypeId == documentTypeId);
    }

    public async Task<bool> IsDocumentExtendedAttributeUsed(int documentExtendedAttributeId)
    {
        return await _repository.Entities.AnyAsync(b => b.ExtendedAttributes.Any(x => x.Id == documentExtendedAttributeId));
    }
}
