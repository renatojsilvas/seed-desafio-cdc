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

        var (code, message) = result.ToEndpointResult();

        await SendAsync(
            message, 
            code,
            cancellation: cancellationToken);
    }
}
