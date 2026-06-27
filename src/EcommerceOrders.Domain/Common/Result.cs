using System.Diagnostics;

namespace EcommerceOrders.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }
    public Error? Error { get; }
    
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error is not null)
            throw new InvalidOperationException("A successful result cannot contain an error.");
        
        if (!isSuccess && error is null)
            throw new InvalidOperationException("A failure result must contain an error.");
        
        IsSuccess = isSuccess;
        Error = error;
    }
    
    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error);
}

public class Result<T> : Result
{
    private readonly T? _value;
    
    public T Value => IsSuccess ? _value! : throw new InvalidOperationException("Cannot access value from a failed result.");

    private Result(T? value, bool isSuccess, Error? error) : base(isSuccess, error)
    {
        _value = value;
    }
    
    public static Result<T> Success(T value) => new (value, true, null);
    public static new Result<T> Failure(Error error) => new(default, false, error);
}