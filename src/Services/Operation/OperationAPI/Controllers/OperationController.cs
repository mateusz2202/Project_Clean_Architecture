using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Operation.Application.Features.Operation.Commands.AddAtributes;
using Operation.Application.Features.Operation.Commands.AddOperation;
using Operation.Application.Features.Operation.Commands.AddOperationWithAtribute;
using Operation.Application.Features.Operation.Commands.ClearCache;
using Operation.Application.Features.Operation.Commands.DeleteAll;
using Operation.Application.Features.Operation.Commands.DeleteAttribute;
using Operation.Application.Features.Operation.Commands.DeleteOperation;
using Operation.Application.Features.Operation.Commands.UpdateOperationWithattribute;
using Operation.Application.Features.Operation.Queries.GetAll.Attributes;
using Operation.Application.Features.Operation.Queries.GetAll.Operations;
using Operation.Application.Features.Operation.Queries.GetAll.OperationsWithAtrributes;
using Operation.Application.Features.Operation.Queries.GetById;

namespace Operation.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class OperationController : ControllerBase
{
    private readonly IMediator _mediator;
    public OperationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet()]
    public async Task<IActionResult> GetAllOperationsWithAtrributes()
        => Ok(await _mediator.Send(new GetAllOperationsWithAtrributesQuery()));

    [HttpGet("operations")]
    public async Task<IActionResult> GetAllOperations()
        => Ok(await _mediator.Send(new GetAllOperationsQuery()));

    [HttpGet("attributes")]
    public async Task<IActionResult> GetAllAttributes()
       => Ok(await _mediator.Send(new GetAllAttributesQuery()));

    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetOperationWithAttributeById([FromRoute] int id)
       => Ok(await _mediator.Send(new GetOperationWithAttributeByIdQuery() { Id = id }));

    [HttpGet("code/{code}")]
    public async Task<IActionResult> GetOperationWithAttributeByCoded([FromRoute] string code)
        => Ok(await _mediator.Send(new GetOperationWithAttributeByCodeQuery() { Code = code }));

    [HttpGet("operations/id/{id}")]
    public async Task<IActionResult> GetOperationById([FromRoute] int id)
        => Ok(await _mediator.Send(new GetOperationByIdQuery() { Id = id }));

    [HttpGet("operations/code/{code}")]
    public async Task<IActionResult> GetOperationByCode([FromRoute] string code)
        => Ok(await _mediator.Send(new GetOperationByCodeQuery() { Code = code }));

    [HttpGet("attributes/id/{id}")]
    public async Task<IActionResult> GetAttributeById([FromRoute] int id)
         => Ok(await _mediator.Send(new GetAttributeByIddQuery() { Id = id }));

    [HttpGet("attributes/code/{code}")]
    public async Task<IActionResult> GetAttributeByCoded([FromRoute] string code)
        => Ok(await _mediator.Send(new GetAttributeByCodeQuery() { Code = code }));

    [HttpPost("operations")]
    public async Task<IActionResult> AddOperation(AddOperationCommand addOperationCommand)
        => Ok(await _mediator.Send(addOperationCommand));

    [HttpPost("attributes")]
    public async Task<IActionResult> AddOperationAttributs(AddOperationAttributCommad attributes)
    {
        await _mediator.Send(attributes);
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> AddOperationWithAttributes(AddOperationWithAttributCommad addOperationWithAttributCommad)
    {
        await _mediator.Send(addOperationWithAttributCommad);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOperationWithAttributes(UpdateOperationWithAttributesCommand operationWithAttributesCommand)
    {
        await _mediator.Send(operationWithAttributesCommand);
        return Ok();
    }

    [HttpDelete("id/{id}")]
    public async Task<IActionResult> DeleteOperationById([FromRoute] int id)
    {
        await _mediator.Send(new DeleteOperationByIdCommand() { Id = id });
        return NoContent();
    }

    [HttpDelete("code/{code}")]
    public async Task<IActionResult> DeleteOperationByCode([FromRoute] string code)
    {
        await _mediator.Send(new DeleteOperationByCodeCommand() { Code = code });
        return NoContent();
    }

    [HttpDelete("attributes/id/{id}")]
    public async Task<IActionResult> DeleteAttributeById([FromRoute] int id)
    {
        await _mediator.Send(new DeleteAttributeByIdCommand() { Id = id });
        return NoContent();
    }

    [HttpDelete("attributes/code/{code}")]
    public async Task<IActionResult> DeleteAttributeByCode([FromRoute] string code)
    {
        await _mediator.Send(new DeleteAttributeByCodeCommand() { Code = code });
        return NoContent();
    }

    [HttpDelete()]
    public async Task<IActionResult> DeleteAll()
    {
        await _mediator.Send(new DeleteAllCommand());
        return NoContent();
    }

    [HttpDelete("cache")]
    public async Task<IActionResult> ClearCache()
    {
        await _mediator.Send(new ClearCahceCommand());
        return NoContent();
    }

}
