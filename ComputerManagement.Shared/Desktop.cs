namespace ComputerManagement.Shared;

public class Desktop : Computer 
{
	public bool HasDualMonitor { get; private set; }

	public Desktop(string name, string brand, string processor, int ramInGb, bool hasDualMonitor = false)
		: base(name, brand, processor, ramInGb) 
	{
		HasDualMonitor = hasDualMonitor;
	}

	public void ToggleDualMonitor() 
	{
		HasDualMonitor = !HasDualMonitor;
		Console.WriteLine($"{Name} dual monitor setup is now: {(HasDualMonitor ? "ON" : "OFF")}.");
	}

	public void UpgradeRam(int additionalRamInGb) 
	{
		RamInGb += additionalRamInGb;
		Console.WriteLine($"{Name} RAM upgraded to {RamInGb}GB.");
	}

	public override string GetSystemInfo() 
	{
		return base.GetSystemInfo() + $" - Dual Monitor: {(HasDualMonitor ? "Yes" : "No")}";
	}
}