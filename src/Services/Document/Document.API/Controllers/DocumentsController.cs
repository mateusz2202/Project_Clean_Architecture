using MediatR;
using Microsoft.AspNetCore.Mvc;
using Document.Application.Features.Documents.Commands.AddEdit;
using Document.Application.Features.Documents.Commands.Delete;
using Document.Application.Features.Documents.Queries.GetAll;
using Document.Application.Features.Documents.Queries.GetById;

namespace Document.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DocumentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentsController(IMediator mediator)
    {
        _mediator = mediator;     
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString)           
        => Ok(await _mediator.Send(new GetAllDocumentsQuery(pageNumber, pageSize, searchString)));   


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _mediator.Send(new GetDocumentByIdQuery(id)));


    [HttpPost]
    public async Task<IActionResult> Post(AddEditDocumentCommand command)
       => Ok(await _mediator.Send(command));


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => Ok(await _mediator.Send(new DeleteDocumentCommand(id)));
}
