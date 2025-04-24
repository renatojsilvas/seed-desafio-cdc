using Application.UseCases;
using FastEndpoints;
using FluentValidation;

namespace WebApi.Endpoints.CreateState;

public class CreateStateValidator : Validator<CreateStateUseCase.CreateStateRequest>
{
    public CreateStateValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name is required")
            .NotEmpty().WithMessage("Name cannot be empty");
        
        RuleFor(x => x.CountryId)
            .NotNull().WithMessage("CountryId is required")
            .NotEmpty().WithMessage("CountryId cannot be empty");
    }
}