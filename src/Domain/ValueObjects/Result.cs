using System.Text.Json.Serialization;

namespace Domain.ValueObjects;

public class Result
{
    public StatusCode StatusCode { get; set; }
    public Error? Error { get; set; }
    public List<Validation>? Validations { get; set; }
    public bool IsSuccess => !HasError && !HasValidations;
    public bool HasValidations => Validations is not null && Validations?.Count > 0;
    public bool HasError => Error is not null;
    
    public bool HasErrors => Error is not null;
    
    public static Result Failure(string message)
        => new()
        {
            Error = new Error(message),
            StatusCode = StatusCode.Error
        };
    
    public static Result Success()
        => new()
        {
            StatusCode = StatusCode.Success
        };
    
    public static Result FailureOnValidations(List<Validation> validations)
        => new()
        {
            Validations = validations,
            StatusCode = StatusCode.BadRequest
        };
    
    public static Result FromResult<TInput>(Result<TInput> input) =>
        new()
        {
            StatusCode = input.StatusCode,
            Error = input.Error,
            Validations = input.Validations ?? []
        };
}

public class Result<T> : Result
{
    public T? Value { get; init; }

    public static Result<T> Success(T value)
        => new()
        {
            Value = value,
            StatusCode = StatusCode.Success
        };
    
    public new static Result<T> Failure(string message)
        => new()
        {
            Error = new Error(message),
            StatusCode = StatusCode.Error
        };
    
    public static Result<T> Failure(Exception exception)
        => new()
        {
            Error = new Error(exception.Message, exception),
            StatusCode = StatusCode.Error
        };

    public new static Result<T> FailureOnValidations(List<Validation> validations)
        => new()
        {
            Validations = validations,
            StatusCode = StatusCode.BadRequest
        };
    
    public static Result<TOutput> FromResult<TInput, TOutput>(Result<TInput> input) =>
        new()
        {
            StatusCode = input.StatusCode,
            Error = input.Error,
            Validations = input.Validations ?? []
        };

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Exception exception) => Failure(exception);
    public static implicit operator Result<T>(List<Validation> validations) => FailureOnValidations(validations);
}

public record Error(string Message, [property: JsonIgnore]Exception? Exception = null);

public record Validation(string Property, string Error)
{
    public override string ToString() => $"Property: {Property}, Error: {Error}";
}

public enum StatusCode
{
    Success = 200,
    NoContent = 204,
    BadRequest = 400,
    NotFound = 404,
    Error = 500,
}