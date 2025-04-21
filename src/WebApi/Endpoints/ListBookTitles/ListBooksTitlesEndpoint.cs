using Application.UseCases;
using FastEndpoints;

namespace WebApi.Endpoints.ListBookTitles;

public class ListBookTitlesEndpoint(ListBookTitlesUseCase listBookTitlesUseCase) 
    : Endpoint<ListBookTitlesUseCase.ListBookTitlesRequest>
{
    public override void Configure()
    {
        Get("/books"); 
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        ListBookTitlesUseCase.ListBookTitlesRequest req, CancellationToken cancellationToken)
    {
        var result = await listBookTitlesUseCase.HandleAsync(req, cancellationToken);

        var (code, message) = result.ToEndpointResult(
            successMessage: result.Data?.BookTitles.Select
                (b => new
                {
                    b.Id, 
                    b.Title
                }));

        await SendAsync(
            message, 
            code,
            cancellation: cancellationToken);
    }
}
