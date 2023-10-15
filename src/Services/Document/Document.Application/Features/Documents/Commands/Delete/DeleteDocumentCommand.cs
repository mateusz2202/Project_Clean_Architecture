using Document.Application.Interfaces.Repositories;
using Document.Shared.Constans;
using Document.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Document.Application.Features.Documents.Commands.Delete;

public record class DeleteDocumentCommand(int Id) : IRequest<Result<int>>;


internal class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, Result<int>>
{
    private readonly IUnitOfWork<int> _unitOfWork;  

    public DeleteDocumentCommandHandler(IUnitOfWork<int> unitOfWork)
    {
        _unitOfWork = unitOfWork;   
    }

    public async Task<Result<int>> Handle(DeleteDocumentCommand command, CancellationToken cancellationToken)
    {
        var documentsWithExtendedAttributes = _unitOfWork.Repository<Domain.Entities.Document>().Entities.Include(x => x.ExtendedAttributes);

        var document = await _unitOfWork.Repository<Domain.Entities.Document>().GetByIdAsync(command.Id);
        if (document != null)
        {
            await _unitOfWork.Repository<Domain.Entities.Document>().DeleteAsync(document);

            // delete all caches related with deleted entity
            var cacheKeys = await documentsWithExtendedAttributes.SelectMany(x => x.ExtendedAttributes).Where(x => x.EntityId == command.Id).Distinct().Select(x => ApplicationConstants.Cache.GetAllEntityExtendedAttributesByEntityIdCacheKey(nameof(Document), x.EntityId))
                .ToListAsync(cancellationToken);
            cacheKeys.Add(ApplicationConstants.Cache.GetAllEntityExtendedAttributesCacheKey(nameof(Document)));
            await _unitOfWork.CommitAndRemoveCache(cancellationToken, cacheKeys.ToArray());

            return await Result<int>.SuccessAsync(document.Id, "Document Deleted");
        }
        else
        {
            return await Result<int>.FailAsync("Document Not Found!");
        }
    }
}
