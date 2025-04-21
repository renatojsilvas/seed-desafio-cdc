using System.Text.Json.Serialization;

namespace Domain.ValueObjects;

public interface IResult
{
    ResultStatus Status { get; }
}

public interface IResult<out T> : IResult
{
    T? Data { get; }
}

public interface IResultValidations : IResult
{
    IEnumerable<Validation> Validations { get; }
}

public interface IResultError : IResult
{
    Error? Error { get; }
}

public interface IRequestEntityWarning : IResult
{
    EntityWarning? EntityWarning { get; }
}


public class Result : IResultError, IRequestEntityWarning, IResultValidations
{
    public bool IsSuccess => Status == ResultStatus.Success;
    
    public static Result Success() => new()
    {
        Status = ResultStatus.Success,
    }; 
    
    public static Result WithError(string message) => new()
    {
        Status = ResultStatus.HasError,
        Error = new Error(message)
    }; 
    
    public static Result WithError(Exception exception) => new()
    {
        Status = ResultStatus.HasError,
        Error = new Error(exception.Message)
    };

    public static Result WithValidations(IEnumerable<Validation> validations) => new()
    {
        Status = ResultStatus.HasValidations,
        Validations = validations
    };
    
    public static Result WithValidations(params Validation[] validations) => new()
    {
        Status = ResultStatus.HasValidations,
        Validations = validations
    };
    
    public static Result WithValidations(string propertyName, string description)
        => WithValidations(new Validation(propertyName, description));

    public static Result WithNoContent() => new()
    {
        Status = ResultStatus.NoContent
    };
    
    public static Result WithEntityNotFound(string entity, object id, string description)
        => new()
        {
            Status = ResultStatus.EntityNotFound,
            EntityWarning = new EntityWarning(entity, id, description)
        };
    
    public static Result WithEntityAlreadyExists(string entity, object id, string description)
        => new()
        {
            Status = ResultStatus.EntityAlreadyExists,
            EntityWarning = new EntityWarning(entity, id, description)
        };
    
    public ResultStatus Status { get; protected set; }
    public Error? Error { get; protected set; }
    public EntityWarning? EntityWarning { get; protected set; }
    public IEnumerable<Validation> Validations { get; protected set; } = [];
}

public class Result<T> : Result, IResult<T>
{
    public T? Data { get; private init; }

    public static Result<T> FromResult(Result data) => new()
    {
        Status = data.Status,
        Error = data.Error,
        EntityWarning = data.EntityWarning,
        Validations = data.Validations
    };
    
    public static Result<T> Success(T data) 
        => new()
        {
            Data = data, 
            Status = ResultStatus.Success
        };
   
    public new static Result<T> WithNoContent() 
        => new()
        {
            Status = ResultStatus.NoContent
        };
    
    public new static Result<T> WithEntityNotFound(string entity, object id, string description)
        => new()
        {
            Status = ResultStatus.EntityNotFound,
            EntityWarning = new EntityWarning(entity, id, description)
        };
    
    public new static Result<T> WithEntityAlreadyExists(string entity, object id, string description)
        => new()
        {
            Status = ResultStatus.EntityAlreadyExists,
            EntityWarning = new EntityWarning(entity, id, description)
        };
    
    public new static Result<T> WithError(string message)
        => new()
        {
            Status = ResultStatus.HasError,
            Error = new Error(message)
        };
    
    public new static Result<T> WithError(Exception exception) 
        => WithError(exception.Message);
    
    public new static Result<T> WithValidations(params Validation[] validations)
        => new()
        {
            Status = ResultStatus.HasValidations,
            Validations = validations
        };
    
    public new static Result<T> WithValidations(string propertyName, string description)
        => WithValidations(new Validation(propertyName, description));
    
    public static implicit operator Result<T>(T data) => Success(data);
    public static implicit operator Result<T>(Exception ex) => WithError(ex);
    public static implicit operator Result<T>(Validation[] validations) => WithValidations(validations);
    public static implicit operator Result<T>(Validation validation) => WithValidations(validation);
}

public record Error(string Message, [property: JsonIgnore]Exception? Exception = null);

public record Validation(string Property, string Error);

public record EntityWarning(string Name, object Id, string Message);

public enum ResultStatus
{
    Success,
    NoContent,
    EntityNotFound,
    EntityAlreadyExists,
    HasValidations,
    HasError,
}