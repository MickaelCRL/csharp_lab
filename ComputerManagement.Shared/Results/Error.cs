namespace ComputerManagement.Shared.Results;

public class Error(string code, string message) : IEquatable<Error>
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public string Code { get; } = code;
    public string Message { get; } = message;

    public bool Equals(Error? other)
    {
        if (other == null)
        {
            return false;
        }

        return other.Code == Code && other.Message == Message;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Error);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Code, Message);
    }
}