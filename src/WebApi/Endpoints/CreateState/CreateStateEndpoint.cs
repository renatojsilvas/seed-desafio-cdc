using Application.UseCases;
using FastEndpoints;

namespace WebApi.Endpoints.CreateState;

public class CreateStateEndpoint(CreateStateUseCase createCountryUseCase) 
    : Endpoint<CreateStateUseCase.CreateStateRequest>
{
    public override void Configure()
    {
        Post("/states"); 
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateStateUseCase.CreateStateRequest req,
        CancellationToken cancellationToken)
    {
        var result = await createCountryUseCase.HandleAsync(req, cancellationToken);

        var (code, message) = result.ToEndpointResult(
            successMessage: new 
                {
                    StateName = result.Data?.Name
                });

        await SendAsync(
            message, 
            code,
            cancellation: cancellationToken);
    }
}
