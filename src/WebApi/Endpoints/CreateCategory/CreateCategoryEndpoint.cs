using Application.UseCases;
using FastEndpoints;
using WebApi.Endpoints.CreateAuthor;

namespace WebApi.Endpoints.CreateCategory;

public class CreateCategorytEndpoint(CreateCategoryUseCase createCategoryUseCase) 
    : Endpoint<CreateCategoryRequest>
{
    public override void Configure()
    {
        Post("/categories"); 
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateCategoryRequest req, CancellationToken cancellationToken)
    {
        var result = await createCategoryUseCase.HandleAsync(req, cancellationToken);

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
