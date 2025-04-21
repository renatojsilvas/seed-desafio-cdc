using Domain.ValueObjects;

namespace WebApi.Endpoints;

public static class Mapper
{
    public static (int Code, object Message) ToEndpointResult<T>(this Result<T> result, object? successMessage)
    {
        if (result.Status == ResultStatus.HasError)
        {
            var message = new
            {
                StatusCode = 500,
                Message = "One or more errors occurred!",
                Errors = result.Error!.Message
            };

            return (Code: 500, Message: message);
        }
        
        if (result.Status == ResultStatus.EntityNotFound)
        {
            var message = new
            {
                StatusCode = 404,
                Message = "One or more errors occurred!",
                Errors = result.EntityWarning!.Message
            };

            return (Code: 404, Message: message);
        }
        
        if (result.Status == ResultStatus.EntityAlreadyExists)
        {
            var message = new
            {
                StatusCode = 409,
                Message = "One or more errors occurred!",
                Errors = result.EntityWarning!.Message
            };

            return (Code: 409, Message: message);
        }
        
        if (result.Status == ResultStatus.HasValidations)
        {
            var message = new
            {
                StatusCode = 400,
                Message = "One or more errors occurred!",
                Errors = result.Validations!
                    .GroupBy(v => v.Property)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(v => v.Error).ToList()
                    )
            };

            return (Code: 400, Message: message);
        }
        
        return (StatusCodes.Status200OK, successMessage!);
    }
}