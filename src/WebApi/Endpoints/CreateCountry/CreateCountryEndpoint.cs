using Application.UseCases;
using FastEndpoints;

namespace WebApi.Endpoints.CreateCountry;

public class CreateCountryEndpoint(CreateCountryUseCase createCountryUseCase) 
    : Endpoint<CreateCountryUseCase.CreateCountryRequest>
{
    public override void Configure()
    {
        Post("/countries"); 
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateCountryUseCase.CreateCountryRequest req,
        CancellationToken cancellationToken)
    {
        var result = await createCountryUseCase.HandleAsync(req, cancellationToken);

        var (code, message) = result.ToEndpointResult(
            successMessage: new 
                {
                    CountryName = result.Data?.Name
                });

        await SendAsync(
            message, 
            code,
            cancellation: cancellationToken);
    }
}
