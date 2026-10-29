using ComputerManagement.API.Events;
using ComputerManagement.Shared;
using ComputerManagement.Shared.Computers;
using ComputerManagement.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace ComputerManagement.API.Services;

public class ComputerService(FleetContext context) : IComputerService
{
    public event EventHandler<ComputerAddedEventArgs>? ComputerAdded;
    public event EventHandler<ComputerUpdatedEventArgs>? ComputerUpdated;
    public event EventHandler<ComputerDeletedEventArgs>? ComputerDeleted;

    public async Task<Result<IEnumerable<Computer>>> GetAllAsync()
    {
        var computers = await context.Computers.ToListAsync();
        return Result.Success<IEnumerable<Computer>>(computers);
    }

    public async Task<Result<IEnumerable<Computer>>> GetAvailableAsync()
    {
        var computers = await context.Computers
            .Where(c => c.Stock > 0)
            .ToListAsync();
        return Result.Success<IEnumerable<Computer>>(computers);
    }

    public async Task<Result<IEnumerable<Computer>>> GetByBrandAsync(string brand)
    {
        var computers = await context.Computers
            .Where(c => c.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
        return Result.Success<IEnumerable<Computer>>(computers);
    }

    public async Task<Result<IEnumerable<Computer>>> GetOrderedByPriceDescAsync()
    {
        var computers = await context.Computers
            .OrderByDescending(c => c.Price)
            .ToListAsync();
        return Result.Success<IEnumerable<Computer>>(computers);
    }

    public async Task<Result<Computer>> GetByIdAsync(Guid id)
    {
        var computer = await context.Computers.FirstOrDefaultAsync(c => c.Id == id);
        if (computer == null)
        {
            return ComputerErrors.CouldNotFoundComputerById;
        }

        return computer;
    }

    public async Task<Result<Computer>> AddAsync(Computer computer)
    {
        context.Computers.Add(computer);
        await context.SaveChangesAsync();

        ComputerAdded?.Invoke(this, new ComputerAddedEventArgs { Computer = computer });
        return computer;
    }

    public async Task<Result<Computer>> UpdateAsync(Guid id, Computer updatedComputer)
    {
        var existing = await context.Computers.FirstOrDefaultAsync(c => c.Id == id);
        if (existing == null)
        {
            return ComputerErrors.CouldNotFoundComputerById;
        }

        context.Entry(existing).CurrentValues.SetValues(updatedComputer);
        await context.SaveChangesAsync();

        ComputerUpdated?.Invoke(this, new ComputerUpdatedEventArgs { Computer = updatedComputer });
        return updatedComputer;
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var computer = await context.Computers.FirstOrDefaultAsync(c => c.Id == id);
        if (computer == null)
        {
            return Result.Failure(ComputerErrors.CouldNotFoundComputerById);
        }

        context.Computers.Remove(computer);
        await context.SaveChangesAsync();

        ComputerDeleted?.Invoke(this, new ComputerDeletedEventArgs { ComputerId = id });
        return Result.Success();
    }
}