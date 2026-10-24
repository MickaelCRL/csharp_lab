using System.Text.Json.Serialization;

namespace ComputerManagement.Shared;

[JsonDerivedType(typeof(Laptop), "laptop")]
[JsonDerivedType(typeof(Desktop), "desktop")]
public abstract class Computer
{
	public Guid Id { get; init; } = Guid.NewGuid();

	public string Name { get; private set; }
	public string Brand { get; private set; }
	public string Processor { get; private set; }
	public int RamInGb { get; protected set; }
	public ComputerState State { get; private set; }

	protected Computer(string name, string brand, string processor, int ramInGb) 
	{
		Name = name;
		Brand = brand;
		Processor = processor;
		RamInGb = ramInGb;
		State = ComputerState.Available;
	}

	public virtual string GetSystemInfo() 
	{
		return $"[{Id}] {Name} ({Brand}) - {Processor}, {RamInGb}GB RAM - Status: {State}";
	}

	public void UpdateState(ComputerState newState) 
	{
		State = newState;
	}
}
