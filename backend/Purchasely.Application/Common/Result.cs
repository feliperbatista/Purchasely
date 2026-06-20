namespace Purchasely.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Value { get; private set; }
    public string[] Errors { get; private set; } = [];
    public int StatusCode { get; private set; }

    public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value, StatusCode = 200 };
    public static Result<T> Failure(int statusCode, params string[] errors) => new() { IsSuccess = false, Errors = errors, StatusCode = statusCode };
}