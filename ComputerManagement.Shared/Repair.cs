using ComputerManagement.Shared.Computers;

namespace ComputerManagement.Shared;

public class Repair
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid ComputerId { get; set; }
    public Computer Computer { get; set; } = null!;
    public string RepairType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = string.Empty;
}