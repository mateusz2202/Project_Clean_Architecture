using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Application.Features.Brands.Commands.AddEdit;
using Product.Application.Application.Features.Brands.Commands.Delete;
using Product.Application.Application.Features.Brands.Commands.Import;
using Product.Application.Application.Features.Brands.Queries.Export;
using Product.Application.Application.Features.Brands.Queries.GetAll;
using Product.Application.Application.Features.Brands.Queries.GetById;

namespace Product.Api.Controllers;

public class BrandsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BrandsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() 
        => Ok(await _mediator.Send(new GetAllBrandsQuery()));


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _mediator.Send(new GetBrandByIdQuery(id)));

    [HttpPost]
    public async Task<IActionResult> Post(AddEditBrandCommand command)
        => Ok(await _mediator.Send(command));


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => Ok(await _mediator.Send(new DeleteBrandCommand(id)));

    [HttpGet("export")]
    public async Task<IActionResult> Export(string searchString = "")
        => Ok(await _mediator.Send(new ExportBrandsQuery(searchString)));


    [HttpPost("import")]
    public async Task<IActionResult> Import(ImportBrandsCommand command)
        => Ok(await _mediator.Send(command));

}