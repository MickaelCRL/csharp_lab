using ComputerManagement.API.Events;
using ComputerManagement.Shared;

namespace ComputerManagement.API.Services;

public interface IComputerService 
{
    event EventHandler<ComputerAddedEventArgs> ComputerAdded;
    event EventHandler<ComputerUpdatedEventArgs> ComputerUpdated;
    event EventHandler<ComputerDeletedEventArgs> ComputerDeleted;

    IEnumerable<Computer> GetAll();
	Computer? GetById(Guid id);
	void Add(Computer computer);
	bool Update(Guid id, Computer updatedComputer);
	bool Delete(Guid id);
	IEnumerable<Computer> GetByBrand(string brand);
}
