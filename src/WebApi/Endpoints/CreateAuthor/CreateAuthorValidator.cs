using Application;
using Application.UseCases;
using Domain;
using FastEndpoints;
using FluentValidation;

namespace WebApi.Endpoints.CreateAuthor;

public class CreateUserValidator : Validator<CreateAuthorRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name is required")
            .NotEmpty().WithMessage("Name cannot be empty");
        
        RuleFor(x => x.Email)
            .NotNull().WithMessage("Email is required")
            .NotEmpty().WithMessage("Email cannot be empty")
            .Must(x => x.Contains('@')).WithMessage("Invalid email address");

        RuleFor(x => x.Description)
            .NotNull()
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(Constants.MaxDescriptionLength)
            .WithMessage($"Description is too long. Max {Constants.MaxDescriptionLength} characters");
    }
}