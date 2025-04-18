using Domain.ValueObjects;

namespace WebApi.Endpoints;

public static class Mapper
{
    public static (int Code, object Message) ToEndpointResult<T>(this Result<T> result, object? successMessage)
    {
        if (result.HasError)
        {
            var message = new
            {
                result.StatusCode,
                Message = "One or more errors occurred!",
                Errors = result.Error!.Message
            };

            return (Code: (int)result.StatusCode, Message: message);
        }
        
        if (result.HasValidations)
        {
            var message = new
            {
                result.StatusCode,
                Message = "One or more errors occurred!",
                Errors = result.Validations!
                    .GroupBy(v => v.Property)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(v => v.Error).ToList()
                    )
            };

            return (Code: (int)result.StatusCode, Message: message);
        }
        
        return (StatusCodes.Status200OK, successMessage!);
    }
}