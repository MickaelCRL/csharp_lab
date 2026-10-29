using Microsoft.AspNetCore.Mvc;
using ComputerManagement.API.Services;
using ComputerManagement.Shared;
using ComputerManagement.Shared.Results;

namespace ComputerManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RepairsController(
    IRepairService repairService,
    IComputerService computerService,
    StockService stockService,
    ILogger<RepairsController> logger) : ControllerBase
{
   
    // POST /api/repairs/simulate
    [HttpPost("simulate")]
    public async Task<IActionResult> SimulateRepair()
    {
        var computersResult = await computerService.GetAllAsync();

        if (!computersResult.IsSuccess)
        {
            return Problem(statusCode: StatusCodes.Status500InternalServerError);
        }

        var computers = computersResult.Value.ToList();

        if (computers.Count < 3)
        {
            return BadRequest(ComputerErrors.InsufficientComputersForSimulation);
        }

        await Task.WhenAll(
            repairService.SimulateRepairAsync(computers[0]),
            repairService.SimulateRepairAsync(computers[1]),
            repairService.SimulateRepairAsync(computers[2])
        );

        return Ok();
    }

    // GET /api/repairs
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Repair>>> GetAllRepairs()
    {
        var result = await repairService.GetAllRepairsAsync();

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }

    // GET /api/repairs/stock-ssd
    [HttpGet("stock-ssd")]
    public ActionResult<int> GetStockSsd()
    {
        Parallel.For(
            0,
            10,
            i => stockService.ConsumeSsd());

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Remaining SSD Stock: {Stock}", stockService.SsdStock);
        }
        return Ok(stockService.SsdStock);
    }
}