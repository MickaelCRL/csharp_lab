namespace ComputerManagement.Shared.Results;

public class Result<TValue> : Result
{
    private readonly TValue _value;

    public TValue Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

    protected internal Result(TValue value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    public static implicit operator Result<TValue>(TValue value) => new(value, true, Error.None);

    public static implicit operator Result<TValue>(Error errorMessage) => Failure<TValue>(errorMessage);
}