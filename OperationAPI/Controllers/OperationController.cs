using Microsoft.AspNetCore.Http;
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

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var operations = await _operationService.GetAll();
        return Ok(operations);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateOperationDTO dto)
    {
        await _operationService.AddOperation(dto);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string code)
    {
        await _operationService.DeleteOperation(code);
        return NoContent();
    }

    [HttpDelete("all")]
    public async Task<IActionResult> DeleteAll()
    {
        await _operationService.DeleteAll();
        return NoContent();
    }
}
