using Application.UseCases;
using FastEndpoints;

namespace WebApi.Endpoints.GetBookDetailsEndpoint;

public class GetBookDetailsValidator : Validator<GetBookDetailUseCase.GetBookDetailsRequest>
{
    public GetBookDetailsValidator()
    {
    }
}