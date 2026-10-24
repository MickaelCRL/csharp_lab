namespace ComputerManagement.API.Events;

public class ComputerDeletedEventArgs : EventArgs
{
    public Guid ComputerId { get; init; }
}

