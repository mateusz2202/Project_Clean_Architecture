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
        await _operationService.AddOperation(code, name);
        return NoContent();
    }
}
