using ComputerManagement.Shared.Computers;

namespace ComputerManagement.API.Events;

public class ComputerAddedEventArgs : EventArgs
{
    public required Computer Computer { get; init; }
}

