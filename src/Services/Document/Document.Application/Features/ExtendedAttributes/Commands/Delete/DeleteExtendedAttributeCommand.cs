using Document.Application.Interfaces.Repositories;
using Document.Domain.Contracts;
using Document.Shared.Constans;
using Document.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Document.Application.Features.ExtendedAttributes.Commands.Delete;

internal class DeleteExtendedAttributeCommandLocalization
{
    // for localization
}

public class DeleteExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute>
    : IRequest<Result<TId>>
        where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
        where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
        where TId : IEquatable<TId>
{
    public TId Id { get; set; }
}

internal class DeleteExtendedAttributeCommandHandler<TId, TEntityId, TEntity, TExtendedAttribute>
    : IRequestHandler<DeleteExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute>, Result<TId>>
        where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
        where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
        where TId : IEquatable<TId>
{  
    private readonly IExtendedAttributeUnitOfWork<TId, TEntityId, TEntity> _unitOfWork;

    public DeleteExtendedAttributeCommandHandler(IExtendedAttributeUnitOfWork<TId, TEntityId, TEntity> unitOfWork)
    {
        _unitOfWork = unitOfWork;    
    }

    public async Task<Result<TId>> Handle(DeleteExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute> command, CancellationToken cancellationToken)
    {
        var extendedAttribute = await _unitOfWork.Repository<TExtendedAttribute>().GetByIdAsync(command.Id);
        if (extendedAttribute != null)
        {
            await _unitOfWork.Repository<TExtendedAttribute>().DeleteAsync(extendedAttribute);

            // delete all caches related with deleted entity extended attribute
            var cacheKeys = await _unitOfWork.Repository<TExtendedAttribute>().Entities.Select(x =>
                ApplicationConstants.Cache.GetAllEntityExtendedAttributesByEntityIdCacheKey(
                    typeof(TEntity).Name, x.Entity.Id)).Distinct().ToListAsync(cancellationToken);
            cacheKeys.Add(ApplicationConstants.Cache.GetAllEntityExtendedAttributesCacheKey(typeof(TEntity).Name));
            await _unitOfWork.CommitAndRemoveCache(cancellationToken, cacheKeys.ToArray());

            return await Result<TId>.SuccessAsync(extendedAttribute.Id, "Extended Attribute Deleted");
        }
        else
        {
            return await Result<TId>.FailAsync("Extended Attribute Not Found!");
        }
    }
}
