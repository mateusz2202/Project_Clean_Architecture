using Document.Application.Interfaces.Repositories;
using Document.Domain.Entities;

namespace Document.Persistence.Repositories;

public class DocumentTypeRepository : IDocumentTypeRepository
{
    private readonly IRepositoryAsync<DocumentType, int> _repository;

    public DocumentTypeRepository(IRepositoryAsync<DocumentType, int> repository)
    {
        _repository = repository;
    }
}
