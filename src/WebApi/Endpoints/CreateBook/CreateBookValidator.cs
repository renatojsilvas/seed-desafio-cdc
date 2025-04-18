using Application;
using Application.UseCases;
using FastEndpoints;
using FluentValidation;

namespace WebApi.Endpoints.CreateBook;

public class CreateBookValidator : Validator<CreateBookRequest>
{
    public CreateBookValidator()
    {
        RuleFor(x => x.Title)
            .NotNull().WithMessage("Title is required")
            .NotEmpty().WithMessage("Title cannot be empty");

        RuleFor(x => x.Summary)
            .NotNull().WithMessage("Summary is required")
            .NotEmpty().WithMessage("Summary cannot be empty")
            .MaximumLength(Constants.MaxSummaryLength)
            .WithMessage($"Summary cannot exceed {Constants.MaxSummaryLength} characters");
        
        RuleFor(x => x.Abstract)
            .NotNull().WithMessage("Abstract is required")
            .NotEmpty().WithMessage("Abstract cannot be empty");

        RuleFor(x => x.Price)
            .NotNull().WithMessage("Price is required")
            .Must(x => x >= Constants.MinPriceValue)
            .WithMessage($"Price must be greater than {Constants.MinPriceValue}");

        RuleFor(x => x.NumberOfPages)
            .NotNull().WithMessage("NumberOfPages is required")
            .Must(x => x >= Constants.MinNumberOfPages)
            .WithMessage($"NumberOfPages must be greater than {Constants.MinNumberOfPages}");
        
        RuleFor(x => x.Isbn)
            .NotNull().WithMessage("ISBN is required")
            .NotEmpty().WithMessage("ISBN cannot be empty");
        
        RuleFor(x => x.PublishDate)
            .NotNull().WithMessage("Publish date is required")
            .Must(x => x > Constants.Today)
                .WithMessage("Publish date be in the future");

        RuleFor(x => x.AuthorId)
            .NotNull().WithMessage("Author is required")
            .Must(x => x > 0).WithMessage("Author Id must be greater than 0");
        
        RuleFor(x => x.CategoryId)
            .NotNull().WithMessage("Category is required")
            .Must(x => x > 0).WithMessage("Category Id must be greater than 0");;
    }
}