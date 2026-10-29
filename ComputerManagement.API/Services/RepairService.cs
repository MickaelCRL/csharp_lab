using ComputerManagement.Shared;
using ComputerManagement.Shared.Computers;
using ComputerManagement.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace ComputerManagement.API.Services;

public class RepairService(IServiceScopeFactory scopeFactory) : IRepairService
{
    public async Task<Result<Repair>> SimulateRepairAsync(Computer computer)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FleetContext>();

        var repair = new Repair
        {
            ComputerId = computer.Id,
            RepairType = "Hardware Maintenance",
            StartDate = DateTime.UtcNow,
            Status = "In Progress"
        };

        context.Repairs.Add(repair);
        await context.SaveChangesAsync();

        await Task.Delay(2000);

        repair.Status = "Completed";
        await context.SaveChangesAsync();

        Console.WriteLine($"{computer.Name} repaired");
        return repair;
    }

    public async Task<Result<IEnumerable<Repair>>> GetAllRepairsAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FleetContext>();

        var repairs = await context.Repairs
            .Include(r => r.Computer)
            .ToListAsync();

        return Result.Success<IEnumerable<Repair>>(repairs);
    }
}