using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OperationAPI.Interfaces;

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
    public async Task<IActionResult> Post(string code, string name)
    {
        await _operationService.AddOperation(name, code);
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
