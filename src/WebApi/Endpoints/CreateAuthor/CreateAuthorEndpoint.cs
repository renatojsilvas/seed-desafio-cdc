using Application.UseCases;
using Domain;
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
        
        await SendAsync(
            new { Author = result.Value }, 
            cancellation: cancellationToken);
    }
}
