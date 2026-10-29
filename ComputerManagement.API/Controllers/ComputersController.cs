using Microsoft.AspNetCore.Mvc;
using ComputerManagement.API.Services;
using ComputerManagement.Shared.Computers;
using ComputerManagement.Shared.Results;

namespace ComputerManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComputersController : ControllerBase
{
	private readonly IComputerService _computerService;
    private readonly ILogger<ComputersController> _logger;
    private readonly IConfiguration _configuration;

	public ComputersController(IComputerService computerService, ILogger<ComputersController> logger, IConfiguration configuration)
    {
        _computerService = computerService;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet("info")]
    public ActionResult<string> GetConfigurationValue() 
    {
        var name = _configuration["Application:Name"];
        var version = _configuration["Application:Version"];
        _logger.LogInformation("Retrieved configuration values: {Name}, {Version}", name, version);
        return Ok(new { Name = name, Version = version });
    }

    // GET /api/computers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Computer>>> GetAll()
    {
        _logger.LogInformation("Getting all computers");
        var result = await _computerService.GetAllAsync();

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }

    // GET /api/computers/available
    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<Computer>>> GetAvailable()
    {
        _logger.LogInformation("Getting available computers (Stock > 0).");
        var result = await _computerService.GetAvailableAsync();

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }

    // GET /api/computers/sorted-by-price
    [HttpGet("sorted-by-price")]
    public async Task<ActionResult<IEnumerable<Computer>>> GetOrderedByPriceDesc()
    {
        _logger.LogInformation("Getting computers ordered by price descending.");
        var result = await _computerService.GetOrderedByPriceDescAsync();

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }

    // GET /api/computers/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Computer>> GetComputerById(Guid id)
    {
        _logger.LogInformation("Getting computer with ID: {Id}", id);
        var result = await _computerService.GetByIdAsync(id);

        if (result.IsSuccess)
        {
            var computer = result.Value;
            return Ok(computer);
        }

        if (result.Error == ComputerErrors.CouldNotFoundComputerById)
            return NotFound();

        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }

    // POST /api/computers
    [HttpPost]
    public async Task<ActionResult<Computer>> Add([FromBody] Computer computer)
    {
        _logger.LogInformation("Adding a new computer with ID {Id}", computer.Id);
        var result = await _computerService.AddAsync(computer);
        
        if (result.IsSuccess)
        {
            var computerCreated = result.Value;
            return CreatedAtAction(nameof(GetComputerById), new { id = computerCreated.Id }, computerCreated);
        }
        
        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }

    // PUT /api/computers/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Computer>> Update(Guid id, [FromBody] Computer computer)
    {
        if (id != computer.Id) return BadRequest();

        _logger.LogInformation("Updating computer with ID {Id}", id);
        var result = await _computerService.UpdateAsync(id, computer);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        if (result.Error == ComputerErrors.CouldNotFoundComputerById)
            return NotFound();

        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }

    // DELETE /api/computers/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation("Deleting computer with ID {Id}", id);
        var result = await _computerService.DeleteAsync(id);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        if (result.Error == ComputerErrors.CouldNotFoundComputerById)
            return NotFound();

        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }

    // GET /api/computers/brand/{brand}
    [HttpGet("brand/{brand}")]
    public async Task<ActionResult<IEnumerable<Computer>>> GetByBrand(string brand)
    {
        _logger.LogInformation("Getting computers by brand: {Brand}", brand);
        var result = await _computerService.GetByBrandAsync(brand);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }
}