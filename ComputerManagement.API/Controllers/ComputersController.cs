using Microsoft.AspNetCore.Mvc;
using ComputerManagement.Shared;
using ComputerManagement.API.Services;

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
    public ActionResult<IEnumerable<Computer>> GetAll()
    {
        _logger.LogInformation("Getting all computers");
        return Ok(_computerService.GetAll());
    }

    // GET /api/computers/{id}
    [HttpGet("{id:guid}")]
    public ActionResult<Computer> GetComputerById(Guid id)
    {
        _logger.LogInformation("Getting computer with ID {Id}", id);
        var computer = _computerService.GetById(id);
        if (computer == null) return NotFound();

        return Ok(computer);
    }

    // POST /api/computers
    [HttpPost]
    public ActionResult Add([FromBody] Computer computer)
    {
        _logger.LogInformation("Adding a new computer with ID {Id}", computer.Id);
        _computerService.Add(computer);
        return CreatedAtAction(nameof(GetComputerById), new { id = computer.Id }, computer);
    }

    // PUT /api/computers/{id}
    [HttpPut("{id:guid}")]
    public ActionResult Update(Guid id, [FromBody] Computer computer)
    {
        if (id != computer.Id) return BadRequest();

        _logger.LogInformation("Updating computer with ID {Id}", id);
        var updated = _computerService.Update(id, computer);
        if (!updated) return NotFound();

        return Ok(computer);
    }

    // DELETE /api/computers/{id}
    [HttpDelete("{id:guid}")]
    public ActionResult Delete(Guid id)
    {
        _logger.LogInformation("Deleting computer with ID {Id}", id);
        var deleted = _computerService.Delete(id);
        if (!deleted) return NotFound();

        return NoContent();
    }

    // GET /api/computers/brand/{brand}
    [HttpGet("brand/{brand}")]
    public ActionResult<IEnumerable<Computer>> GetByBrand(string brand)
    {
		_logger.LogInformation("Getting computers by brand: {Brand}", brand);
		var computers = _computerService.GetByBrand(brand);
        return Ok(computers);
    }
}