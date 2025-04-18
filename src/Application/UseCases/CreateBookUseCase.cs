using Application.Repositories.Repositories;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.UseCases;

public record CreateBookRequest(
    string Title,
    string Summary,
    string Abstract,
    decimal Price,
    uint NumberOfPages,
    string Isbn,
    DateOnly PublishDate,
    int AuthorId,
    int CategoryId);
public record CreateBookResponse(string Name);

public class CreateBookUseCase(
    IBookRepository bookRepository,
    ICategoryRepository categoryRepository,
    IAuthorRepository authorRepository)
{
    public async Task<Result<CreateBookResponse>> HandleAsync(
        CreateBookRequest request, 
        CancellationToken cancellationToken)
    {
        var ensureUniquenessResult = await EnsureUniquenessAsync(request, cancellationToken);
        if (!ensureUniquenessResult.IsSuccess)
            return Result<CreateBookResponse>.FromResult<bool, CreateBookResponse>(ensureUniquenessResult);
        
        var ensureRelationsExistsResult = await EnsureRelationsExistsAsync(request, cancellationToken);
        if (!ensureRelationsExistsResult.IsSuccess)
            return Result<CreateBookResponse>.FromResult<bool, CreateBookResponse>(ensureRelationsExistsResult);
        
        var createBookResult = Book.Create(request.Title, request.Summary, request.Abstract, request.Price,
            request.NumberOfPages, request.Isbn, request.PublishDate, request.AuthorId, request.CategoryId);
        if (!createBookResult.IsSuccess)
            return Result<CreateBookResponse>.FromResult<Book, CreateBookResponse>(createBookResult);
        
        var addCategoryResult = await bookRepository.AddAsync(createBookResult.Value!, cancellationToken);
        if (!addCategoryResult.IsSuccess)
            return Result<CreateBookResponse>.FromResult<Book, CreateBookResponse>(addCategoryResult);

        return new CreateBookResponse(
            createBookResult.Value!.Title);
    }

    private async Task<Result<bool>> EnsureUniquenessAsync(CreateBookRequest request, CancellationToken cancellationToken)
    {
        var validations = new List<Validation>();
        
        var isTitleUniqueResult = await bookRepository.IsUniqueAsync(nameof(request.Title), request.Title, cancellationToken);
        if (!isTitleUniqueResult.IsSuccess)
            return isTitleUniqueResult;
        
        if (!isTitleUniqueResult.Value)
            validations.Add(new Validation(nameof(request.Title), $"{request.Title} already exists"));
        
        var isIsbnUniqueResult = await bookRepository.IsUniqueAsync(nameof(request.Isbn), request.Isbn, cancellationToken);
        if (!isIsbnUniqueResult.IsSuccess)
            return isIsbnUniqueResult;
        
        if (!isIsbnUniqueResult.Value)
            validations.Add(new Validation(nameof(request.Isbn), $"{request.Isbn} already exists"));
        
        if (validations.Any())
            return Result<bool>.FailureOnValidations(validations);

        return true;
    }
    
    private async Task<Result<bool>> EnsureRelationsExistsAsync(CreateBookRequest request, CancellationToken cancellationToken)
    {
        var validations = new List<Validation>();
        
        var authorExistsResult = await authorRepository.ExistsAsync(request.AuthorId, cancellationToken);
        if (!authorExistsResult.IsSuccess)
            return authorExistsResult;
        
        var authorExists = authorExistsResult.Value!;
        if (!authorExists)
            validations.Add(new Validation(nameof(request.AuthorId), $"Author with {request.AuthorId} does not exists"));
        
        var categoryExistsResult = await categoryRepository.ExistsAsync(request.CategoryId, cancellationToken);
        if (!categoryExistsResult.IsSuccess)
            return categoryExistsResult;
        
        var categoryExists = categoryExistsResult.Value!;
        if (!categoryExists)
            validations.Add(new Validation(nameof(request.CategoryId), $"Category with {request.CategoryId} does not exists"));
        
        if (validations.Any())
            return Result<bool>.FailureOnValidations(validations);

        return validations;
    }
}