using Application.UseCases;
using FastEndpoints;

namespace WebApi.Endpoints.CreateAuthor;

public class CreateUserEndpoint(CreateAuthorUseCase createAuthorUseCase) 
    : Endpoint<CreateAuthorRequest>
{
    public override void Configure()
    {
        Post("/authors"); 
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateAuthorRequest req, CancellationToken cancellationToken)
    {
        var result = await createAuthorUseCase.HandleAsync(req, cancellationToken);

        var (code, message) = result.ToEndpointResult(
            successMessage: new
                {
                    result.Data?.Name,
                    result.Data?.Email,
                    result.Data?.Description
                });

        await SendAsync(
            message, 
            code,
            cancellation: cancellationToken);
    }
}
