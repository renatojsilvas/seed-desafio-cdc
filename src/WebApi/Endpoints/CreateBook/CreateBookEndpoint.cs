using Application.UseCases;
using FastEndpoints;

namespace WebApi.Endpoints.CreateBook;

public class CreateBookEndpoint(CreateBookUseCase createBookUseCase) 
    : Endpoint<CreateBookRequest>
{
    public override void Configure()
    {
        Post("/books"); 
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBookRequest req, CancellationToken cancellationToken)
    {
        var result = await createBookUseCase.HandleAsync(req, cancellationToken);

        var (code, message) = result.ToEndpointResult(
            successMessage: new 
                {
                    result.Data?.Name
                });

        await SendAsync(
            message, 
            code,
            cancellation: cancellationToken);
    }
}
