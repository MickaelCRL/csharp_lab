using System.Collections;

namespace ComputerManagement.Shared.Computers;

public class Laptop : Computer 
{
	public int BatteryLevel { get; private set; }

	public Laptop(string name, string brand, string processor, int ramInGb, int batteryLevel = 100, int stock = 5, decimal price = 1000m)
		: base(name, brand, processor, ramInGb, stock, price)
    {
		BatteryLevel = batteryLevel;
	}

	public void ChargeBattery(int percentage) 
	{
		BatteryLevel = Math.Min(100, BatteryLevel + percentage);
		Console.WriteLine($"{Name} battery charged to {BatteryLevel}%.");
	}

	public void UseOnBattery(int percentageUsed) 
	{
		BatteryLevel = Math.Max(0, BatteryLevel - percentageUsed);
		if (BatteryLevel == 0) {
			UpdateState(ComputerState.Broken);
		}
	}

	public override string GetSystemInfo() 
	{
		return base.GetSystemInfo() + $" - Battery: {BatteryLevel}%";
	}
}