namespace Domain;

public class Result
{
    public StatusCode StatusCode { get; set; }
    public Error? Error { get; set; }
    public List<Validation>? Validations { get; set; }
    
    public bool IsSuccess => StatusCode is StatusCode.Success;
    public bool IsSuccessOrNoContentOrCreated => StatusCode is StatusCode.Success or StatusCode.Created or StatusCode.NoContent;
    public bool HasValidations => StatusCode is StatusCode.HasValidations;
}

public class Result<T> : Result
{
    public T? Value { get; init; }

    private static Result<T> Success(T value)
        => new()
        {
            Value = value,
            StatusCode = StatusCode.Success
        };
    
    public static Result<T> Failure(string message)
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

    public static Result<T> FailureOnValidations(List<Validation> validations)
        => new()
        {
            Validations = validations,
            StatusCode = StatusCode.HasValidations
        };

    public static Result<T> EntityNotFound(string message)
        => new()
        {
            StatusCode = StatusCode.NotFound,
            Error = new Error(message)
        };
    
    public static Result<T> NoContent(T value)
        => new()
        {
            Value = value,
            StatusCode = StatusCode.NoContent
        };
    
    public static Result<TOutput> FromResult<TInput, TOutput>(Result<TInput> input) =>
        new()
        {
            StatusCode = input.StatusCode,
            Error = input.Error,
            Validations = input.Validations ?? []
        };

    public static Result<T> Created(T value)
        => new()
        {
            StatusCode = StatusCode.Created,
            Value = value
        };

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Exception exception) => Failure(exception);
    public static implicit operator Result<T>(List<Validation> validations) => FailureOnValidations(validations);
}

public record Error(string Message, Exception? Exception = null);

public record Validation(string Property, string Error)
{
    public override string ToString() => $"Property: {Property}, Error: {Error}";
}

public enum StatusCode
{
    Success = 200,
    Created = 201,
    NoContent = 204,
    HasValidations = 400,
    NotFound = 404,
    Error = 500,
}