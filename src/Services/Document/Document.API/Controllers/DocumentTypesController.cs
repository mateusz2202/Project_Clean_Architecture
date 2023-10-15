using Document.Application.Features.DocumentTypes.Commands.AddEdit;
using Document.Application.Features.DocumentTypes.Commands.Delete;
using Document.Application.Features.DocumentTypes.Queries.Export;
using Document.Application.Features.DocumentTypes.Queries.GetAll;
using Document.Application.Features.DocumentTypes.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Document.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DocumentTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllDocumentTypesQuery()));


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _mediator.Send(new GetDocumentTypeByIdQuery(id)));


    [HttpPost]
    public async Task<IActionResult> Post(AddEditDocumentTypeCommand command)
        => Ok(await _mediator.Send(command));


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => Ok(await _mediator.Send(new DeleteDocumentTypeCommand(id)));


    [HttpGet("export")]
    public async Task<IActionResult> Export(string searchString = "")
        => Ok(await _mediator.Send(new ExportDocumentTypesQuery(searchString)));

}
