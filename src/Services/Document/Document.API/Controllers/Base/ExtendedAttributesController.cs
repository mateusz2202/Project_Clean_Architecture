using Document.Application.Features.ExtendedAttributes.Commands.AddEdit;
using Document.Application.Features.ExtendedAttributes.Commands.Delete;
using Document.Application.Features.ExtendedAttributes.Queries.Export;
using Document.Application.Features.ExtendedAttributes.Queries.GetAll;
using Document.Application.Features.ExtendedAttributes.Queries.GetAllByEntityId;
using Document.Application.Features.ExtendedAttributes.Queries.GetById;
using Document.Domain.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Document.API.Controllers.Base;


[Route("api/[controller]")]
[ApiController]
public abstract class ExtendedAttributesController<TId, TEntityId, TEntity, TExtendedAttribute> :ControllerBase
        where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
        where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
        where TId : IEquatable<TId>
{
    private readonly IMediator _mediator;

    public ExtendedAttributesController(IMediator mediator)
    {
        _mediator = mediator;
    }

 
    [HttpGet]
    public virtual async Task<IActionResult> GetAll()
    {
        var extendedAttributes = await _mediator.Send(new GetAllExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute>());
        return Ok(extendedAttributes);
    }


    [HttpGet("by-entity/{entityId}")]
    public virtual async Task<IActionResult> GetAllByEntityId(TEntityId entityId)
    {
        var extendedAttributes = await _mediator.Send(new GetAllExtendedAttributesByEntityIdQuery<TId, TEntityId, TEntity, TExtendedAttribute>(entityId));
        return Ok(extendedAttributes);
    }

   
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetById(TId id)
    {
        var extendedAttribute = await _mediator.Send(new GetExtendedAttributeByIdQuery<TId, TEntityId, TEntity, TExtendedAttribute> { Id = id });
        return Ok(extendedAttribute);
    }

  
    [HttpPost]
    public virtual async Task<IActionResult> Post(AddEditExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute> command)
    {
        return Ok(await _mediator.Send(command));
    }


    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(TId id)
    {
        return Ok(await _mediator.Send(new DeleteExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute> { Id = id }));
    }


    [HttpGet("export")]
    public virtual async Task<IActionResult> Export(string searchString = "", TEntityId entityId = default, bool includeEntity = false, bool onlyCurrentGroup = false, string currentGroup = "")
    {
        return Ok(await _mediator.Send(new ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute>(searchString, entityId, includeEntity, onlyCurrentGroup, currentGroup)));
    }
}
