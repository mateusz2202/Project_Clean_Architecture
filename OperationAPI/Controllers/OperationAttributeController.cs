using Microsoft.AspNetCore.Mvc;
using OperationAPI.Interfaces;


namespace OperationAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class OperationAttributeController : ControllerBase
{
    private readonly IOperationService _operationService;
    public OperationAttributeController(IOperationService operationService)
    {
        _operationService = operationService;
    }

    [HttpPost]
    public async Task<IActionResult> AddOperationAttributs([FromBody] object attributes)
    {
        await _operationService.AddAttributes(attributes);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _operationService.GetAllAttribute();
        return Ok(result);
    }
}
