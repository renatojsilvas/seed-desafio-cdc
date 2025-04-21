using Application.UseCases;
using FastEndpoints;

namespace WebApi.Endpoints.GetBookDetailsEndpoint;

public class GetBookDetailsEndpoint(GetBookDetailUseCase getBookDetailUseCase) 
    : Endpoint<GetBookDetailUseCase.GetBookDetailsRequest>
{
    public override void Configure()
    {
        Get("/books/{id:int}"); 
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        GetBookDetailUseCase.GetBookDetailsRequest req, CancellationToken cancellationToken)
    {
        var result = await getBookDetailUseCase.HandleAsync(req, cancellationToken);

        var (code, message) = result.ToEndpointResult(
            successMessage: result.Data?.Book);

        await SendAsync(
            message, 
            code,
            cancellation: cancellationToken);
    }
}
