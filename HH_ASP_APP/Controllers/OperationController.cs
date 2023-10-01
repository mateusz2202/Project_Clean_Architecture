using HH_ASP_APP.Hubs;
using HH_ASP_APP.Interfaces;
using HH_ASP_APP.Models;
using Microsoft.AspNetCore.Mvc;

namespace HH_ASP_APP.Controllers;

public class OperationController : Controller
{
    private readonly IOperationServices _operationServices;   

    public OperationController(IOperationServices operationServices)
    {
        _operationServices = operationServices;     
    }

    public async Task<IActionResult> Index()
    {
        var operations = await _operationServices.GetOperations();      
        return View(operations);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Operation model)
    {
        await _operationServices.CreateOperation(model);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var operation = await _operationServices.GetOperationById(id);
        return View(operation);
    }
    public async Task<IActionResult> Delete(int id)
    {
        var operation = await _operationServices.GetOperationById(id);
        return View(operation);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id, IFormCollection collection)
    {
        await _operationServices.DeleteOperationById(id);
        return RedirectToAction(nameof(Index));
    }
   
}
