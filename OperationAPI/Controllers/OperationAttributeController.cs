using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using OperationAPI.Interfaces;
using System.Text.Json.Nodes;

namespace OperationAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class OperationAttributeController : ControllerBase
{
    private readonly IOperationAttributeService _operationAttributeService;
    public OperationAttributeController(IOperationAttributeService operationAttributeService)
    {
        _operationAttributeService = operationAttributeService;
    }

    [HttpPost]
    public async Task<IActionResult> AddOperationAttributs([FromBody] object attributes)
    {
        await _operationAttributeService.AddAttributes(attributes);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _operationAttributeService.GetAll();
        return Ok(result);
    }
}
