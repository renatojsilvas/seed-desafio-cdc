using Application.UseCases;
using FastEndpoints;
using FluentValidation;

namespace WebApi.Endpoints.CreateCountry;

public class CreateCountryValidator : Validator<CreateCountryUseCase.CreateCountryRequest>
{
    public CreateCountryValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name is required")
            .NotEmpty().WithMessage("Name cannot be empty");
    }
}