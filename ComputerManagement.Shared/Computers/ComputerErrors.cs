namespace ComputerManagement.Shared.Results;

public static class ComputerErrors
{
    public static Error CouldNotFoundComputerById =
        new("CouldNotFoundComputerById", $"Could not find computer by id.");
    public static readonly Error InsufficientComputersForSimulation =
        new("Repair.InsufficientComputers", "At least 3 computers are required to run the repair simulation.");
}