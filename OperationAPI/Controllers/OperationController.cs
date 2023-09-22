using Microsoft.AspNetCore.Mvc;
using OperationAPI.Interfaces;
using OperationAPI.Models;

namespace OperationAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class OperationController : ControllerBase
{
    private readonly IOperationService _operationService;
    public OperationController(IOperationService operationService)
    {
        _operationService = operationService;
    }

    [HttpGet("operations")]
    public async Task<IActionResult> GetAllOperations()
    {
        var operations = await _operationService.GetAllOperations();
        return Ok(operations);
    }

    [HttpGet("attributes")]
    public async Task<IActionResult> GetAllAttributes()
    {
        var result = await _operationService.GetAllAttributes();
        return Ok(result);
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllOperationsWithAtrributes()
    {
        var operations = await _operationService.GetAllOperationsWithAtrributes();
        return Ok(operations);
    }

    [HttpGet("operations/id/{id}")]
    public async Task<IActionResult> GetOperationById([FromRoute] int id)
    {
        var operation = await _operationService.GetOperationById(id);
        return Ok(operation);
    }

    [HttpGet("operations/code/{code}")]
    public async Task<IActionResult> GetOperationByCode([FromRoute] string code)
    {
        var operation = await _operationService.GetOperationByCode(code);
        return Ok(operation);
    }

    [HttpGet("attributes/id/{id}")]
    public async Task<IActionResult> GetAttributeById([FromRoute] int id)
    {
        var operation = await _operationService.GetAttributeById(id);
        return Ok(operation);
    }

    [HttpGet("attributes/code/{code}")]
    public async Task<IActionResult> GetAttributeByCoded([FromRoute] string code)
    {
        var operation = await _operationService.GetAttributeByCoded(code);
        return Ok(operation);
    }
    
    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetOperationWithAttributeById([FromRoute] int id)
    {
        var operation = await _operationService.GetOperationWithAttributeById(id);
        return Ok(operation);
    }

    [HttpGet("code/{code}")]
    public async Task<IActionResult> GetOperationWithAttributeByCoded([FromRoute] string code)
    {
        var operation = await _operationService.GetOperationWithAttributeByCoded(code);
        return Ok(operation);
    }

    [HttpPost("operations")]
    public async Task<IActionResult> AddOperation(CreateOperationDTO dto)
    {
        await _operationService.AddOperation(dto);
        return NoContent();
    }

    [HttpPost("attributes")]
    public async Task<IActionResult> AddOperationAttributs([FromBody] object attributes)
    {
        await _operationService.AddAttributes(attributes);
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> AddOperationWithAttributes(CreateOperationWithAttributeDTO dto)
    {
        await _operationService.AddOperationWithAttributes(dto);
        return NoContent();
    }

    [HttpDelete("id/{id}")]
    public async Task<IActionResult> DeleteOperationById([FromRoute] int id)
    {
        await _operationService.DeleteOperationById(id);
        return NoContent();
    }

    [HttpDelete("code/{code}")]
    public async Task<IActionResult> DeleteOperationByCode([FromRoute] string code)
    {
        await _operationService.DeleteOperationByCode(code);
        return NoContent();
    }

    [HttpDelete("attributes/id/{id}")]
    public async Task<IActionResult> DeleteAttributeById([FromRoute] int id)
    {
        await _operationService.DeleteAttributeById(id);
        return NoContent();
    }

    [HttpDelete("attributes/code/{code}")]
    public async Task<IActionResult> DeleteAttributeByCode([FromRoute] string code)
    {
        await _operationService.DeleteAttributeByCode(code);
        return NoContent();
    }

    [HttpDelete()]
    public async Task<IActionResult> DeleteAll()
    {
        await _operationService.DeleteAll();
        return NoContent();
    }

    [HttpDelete("cache")]
    public async Task<IActionResult> ClearCache()
    {
        await _operationService.ClearCache();
        return NoContent();
    }
}
