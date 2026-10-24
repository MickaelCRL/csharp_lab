using ComputerManagement.Shared;

namespace ComputerManagement.API.Events;

public class ComputerAddedEventArgs : EventArgs
{
    public required Computer Computer { get; init; }
}

