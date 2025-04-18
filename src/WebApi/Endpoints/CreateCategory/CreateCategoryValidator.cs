using Application.UseCases;
using Domain;
using FastEndpoints;
using FluentValidation;

namespace WebApi.Endpoints.CreateCategory;

public class CreateUserValidator : Validator<CreateCategoryRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name is required")
            .NotEmpty().WithMessage("Name cannot be empty");
    }
}