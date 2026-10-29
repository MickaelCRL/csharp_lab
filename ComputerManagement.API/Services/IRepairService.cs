using ComputerManagement.Shared;
using ComputerManagement.Shared.Computers;
using ComputerManagement.Shared.Results;

namespace ComputerManagement.API.Services
{
    public interface IRepairService
    {
        Task<Result<Repair>> SimulateRepairAsync(Computer computer);
        Task<Result<IEnumerable<Repair>>> GetAllRepairsAsync();
    }
}
