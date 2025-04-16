using Application.UseCases;
using Domain;
using Domain.ValueObjects;

namespace WebApi.Endpoints.CreateAuthor;

public static class CreateAuthorResponseMapper
{
    public static (int Code, object Message) ToEndpointResult(this Result<CreateAuthorResponse> result)
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
        
        return (StatusCodes.Status200OK, 
            new
            {
                result.Value!.Name,
                result.Value!.Email,
                result.Value!.Description
            });
    }
}