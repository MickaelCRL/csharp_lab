using ComputerManagement.API.Events;
using ComputerManagement.Shared.Computers;
using ComputerManagement.Shared.Results;

namespace ComputerManagement.API.Services;

public interface IComputerService 
{
    event EventHandler<ComputerAddedEventArgs> ComputerAdded;
    event EventHandler<ComputerUpdatedEventArgs> ComputerUpdated;
    event EventHandler<ComputerDeletedEventArgs> ComputerDeleted;

    Task<Result<IEnumerable<Computer>>> GetAllAsync();
    Task<Result<IEnumerable<Computer>>> GetAvailableAsync();
    Task<Result<IEnumerable<Computer>>> GetByBrandAsync(string brand);
    Task<Result<IEnumerable<Computer>>> GetOrderedByPriceDescAsync();
    Task<Result<Computer>> GetByIdAsync(Guid id);
    Task<Result<Computer>> AddAsync(Computer computer);
    Task<Result<Computer>> UpdateAsync(Guid id, Computer updatedComputer);
    Task<Result> DeleteAsync(Guid id);
}
