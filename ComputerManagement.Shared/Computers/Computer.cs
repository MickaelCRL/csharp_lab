using System.Collections;
using System.Text.Json.Serialization;

namespace ComputerManagement.Shared.Computers;

[JsonDerivedType(typeof(Laptop), "laptop")]
[JsonDerivedType(typeof(Desktop), "desktop")]
public abstract class Computer
{
	public Guid Id { get; init; } = Guid.NewGuid();

	public string Name { get; private set; }
	public string Brand { get; private set; }
	public string Processor { get; private set; }
	public int RamInGb { get; protected set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public ComputerState State { get; private set; }

	protected Computer(string name, string brand, string processor, int ramInGb, int stock = 5, decimal price = 1000m) 
	{
		Name = name;
		Brand = brand;
		Processor = processor;
		RamInGb = ramInGb;
        Stock = stock;
        Price = price;
        State = ComputerState.Available;
	}

	public virtual string GetSystemInfo() 
	{
        return $"[{Id}] {Name} ({Brand}) - {Processor}, {RamInGb}GB RAM - Stock: {Stock} - Price: {Price} - Status: {State}";
    }

	public void UpdateState(ComputerState newState) 
	{
		State = newState;
	}
}
