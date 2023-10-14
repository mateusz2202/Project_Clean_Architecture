using AutoMapper;
using Document.Application.Interfaces.Repositories;
using Document.Domain.Entities;
using Document.Shared.Constans;
using Document.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace Document.Application.Features.DocumentTypes.Commands.AddEdit;

public class AddEditDocumentTypeCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
}

internal class AddEditDocumentTypeCommandHandler : IRequestHandler<AddEditDocumentTypeCommand, Result<int>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork<int> _unitOfWork;

    public AddEditDocumentTypeCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(AddEditDocumentTypeCommand command, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Repository<DocumentType>().Entities.Where(p => p.Id != command.Id)
            .AnyAsync(p => p.Name == command.Name, cancellationToken))
        {
            return await Result<int>.FailAsync("Document type with this name already exists.");
        }

        if (command.Id == 0)
        {
            var documentType = _mapper.Map<DocumentType>(command);
            await _unitOfWork.Repository<DocumentType>().AddAsync(documentType);
            await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDocumentTypesCacheKey);
            return await Result<int>.SuccessAsync(documentType.Id, "Document Type Saved");
        }
        else
        {
            var documentType = await _unitOfWork.Repository<DocumentType>().GetByIdAsync(command.Id);
            if (documentType != null)
            {
                documentType.Name = command.Name ?? documentType.Name;
                documentType.Description = command.Description ?? documentType.Description;
                await _unitOfWork.Repository<DocumentType>().UpdateAsync(documentType);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDocumentTypesCacheKey);
                return await Result<int>.SuccessAsync(documentType.Id, "Document Type Updated");
            }
            else
            {
                return await Result<int>.FailAsync("Document Type Not Found!");
            }
        }
    }
}
