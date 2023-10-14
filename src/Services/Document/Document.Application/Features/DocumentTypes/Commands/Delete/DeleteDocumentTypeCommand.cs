using Document.Application.Interfaces.Repositories;
using Document.Domain.Entities;
using Document.Shared.Constans;
using Document.Shared.Wrapper;
using MediatR;

namespace Document.Application.Features.DocumentTypes.Commands.Delete;

public record DeleteDocumentTypeCommand(int Id) : IRequest<Result<int>>;

internal class DeleteDocumentTypeCommandHandler : IRequestHandler<DeleteDocumentTypeCommand, Result<int>>
{
    private readonly IDocumentRepository _documentRepository; 
    private readonly IUnitOfWork<int> _unitOfWork;

    public DeleteDocumentTypeCommandHandler(IUnitOfWork<int> unitOfWork, IDocumentRepository documentRepository)
    {
        _unitOfWork = unitOfWork;
        _documentRepository = documentRepository;  
    }

    public async Task<Result<int>> Handle(DeleteDocumentTypeCommand command, CancellationToken cancellationToken)
    {
        var isDocumentTypeUsed = await _documentRepository.IsDocumentTypeUsed(command.Id);
        if (!isDocumentTypeUsed)
        {
            var documentType = await _unitOfWork.Repository<DocumentType>().GetByIdAsync(command.Id);
            if (documentType != null)
            {
                await _unitOfWork.Repository<DocumentType>().DeleteAsync(documentType);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDocumentTypesCacheKey);
                return await Result<int>.SuccessAsync(documentType.Id, "Document Type Deleted");
            }
            else
            {
                return await Result<int>.FailAsync("Document Type Not Found!");
            }
        }
        else
        {
            return await Result<int>.FailAsync("Deletion Not Allowed");
        }
    }
}