using ComputerManagement.API.Events;
using ComputerManagement.Shared;

namespace ComputerManagement.API.Services;

public class ComputerService : IComputerService
{
	private readonly FleetContext _context;
    public event EventHandler<ComputerAddedEventArgs>? ComputerAdded;
    public event EventHandler<ComputerUpdatedEventArgs>? ComputerUpdated;
    public event EventHandler<ComputerDeletedEventArgs>? ComputerDeleted;

    public ComputerService(FleetContext context)
	{
		_context = context;
	}

	public IEnumerable<Computer> GetAll() 
	{
		return _context.Computers.ToList();
	}

	public Computer? GetById(Guid id) 
	{
		return _context.Computers.FirstOrDefault(c => c.Id == id);
	}

	public void Add(Computer computer) 
	{
		_context.Computers.Add(computer);
		_context.SaveChanges();

        ComputerAdded?.Invoke(this, new ComputerAddedEventArgs { Computer = computer });
    }

	public bool Update(Guid id, Computer updatedComputer) {
		var computer = GetById(id);
		if (computer == null) return false;

		_context.Entry(computer).CurrentValues.SetValues(updatedComputer);
		_context.SaveChanges();

        ComputerUpdated?.Invoke(this, new ComputerUpdatedEventArgs { Computer = updatedComputer });

        return true;
	}

	public bool Delete(Guid id) 
	{
		var computer = GetById(id);
		if (computer == null) return false;

		_context.Computers.Remove(computer);
		_context.SaveChanges();

        ComputerDeleted?.Invoke(this, new ComputerDeletedEventArgs { ComputerId = id });

        return true;
	}

    public IEnumerable<Computer> GetByBrand(string brand)
    {
        return _context.Computers.Where(c => c.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase));
    }
}