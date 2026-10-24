using ComputerManagement.Shared;

namespace ComputerManagement.API.Events;

public class ComputerUpdatedEventArgs
{
    public required Computer Computer { get; init; }
}
