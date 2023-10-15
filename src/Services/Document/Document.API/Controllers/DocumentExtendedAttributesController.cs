using Document.API.Controllers.Base;
using Document.Application.Features.ExtendedAttributes.Commands.AddEdit;
using Document.Domain.ExtendedAttributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Document.API.Controllers;

public class DocumentExtendedAttributesController : ExtendedAttributesController<int, int,Domain.Entities.Document, DocumentExtendedAttribute>
{
    public DocumentExtendedAttributesController(IMediator mediator) : base(mediator)
    {
    }

    public override Task<IActionResult> GetAll()
    {
        return base.GetAll();
    }


    public override Task<IActionResult> GetAllByEntityId(int entityId)
    {
        return base.GetAllByEntityId(entityId);
    }


    public override Task<IActionResult> GetById(int id)
    {
        return base.GetById(id);
    }

  
    public override Task<IActionResult> Post(AddEditExtendedAttributeCommand<int, int, Domain.Entities.Document, DocumentExtendedAttribute> command)
    {
        return base.Post(command);
    }

 
    public override Task<IActionResult> Delete(int id)
    {
        return base.Delete(id);
    }


    public override Task<IActionResult> Export(string searchString = "", int entityId = default, bool includeEntity = false, bool onlyCurrentGroup = false, string currentGroup = "")
    {
        return base.Export(searchString, entityId, includeEntity, onlyCurrentGroup, currentGroup);
    }
}
