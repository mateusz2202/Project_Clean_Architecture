using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Application.Features.Products.Commands.AddEdit;
using Product.Application.Application.Features.Products.Commands.Delete;
using Product.Application.Application.Features.Products.Queries.Export;
using Product.Application.Application.Features.Products.Queries.GetAllPaged;
using Product.Application.Application.Features.Products.Queries.GetProductImage;

namespace Product.Api.Controllers;

public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        => Ok(await _mediator.Send(new GetAllProductsQuery(pageNumber, pageSize, searchString, orderBy)));


    [HttpGet("image/{id}")]
    public async Task<IActionResult> GetProductImageAsync(int id)
        => Ok(await _mediator.Send(new GetProductImageQuery(id)));



    [HttpPost]
    public async Task<IActionResult> Post(AddEditProductCommand command)
        => Ok(await _mediator.Send(command));



    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => Ok(await _mediator.Send(new DeleteProductCommand(id)));



    [HttpGet("export")]
    public async Task<IActionResult> Export(string searchString = "")
        => Ok(await _mediator.Send(new ExportProductsQuery(searchString)));

}
