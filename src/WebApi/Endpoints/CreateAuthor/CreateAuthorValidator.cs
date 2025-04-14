using Application.UseCases;
using FastEndpoints;
using FluentValidation;

namespace WebApi.Endpoints.CreateAuthor;

public class CreateUserValidator : Validator<CreateAuthorRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name is required")
            .NotEmpty().WithMessage("Name is required");
        
        RuleFor(x => x.Email)
            .NotNull().WithMessage("Email is required")
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");
        
        RuleFor(x => x.Description)
            .NotNull().WithMessage("Description is required")
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(400).WithMessage("Description is too long. Max 400 characters");
    }
}