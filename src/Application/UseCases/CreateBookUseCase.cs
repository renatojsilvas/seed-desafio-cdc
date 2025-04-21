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
            return Result<CreateBookResponse>.FromResult(ensureUniquenessResult);
        
        var ensureRelationsExistsResult = await EnsureRelationsExistsAsync(request, cancellationToken);
        if (!ensureRelationsExistsResult.IsSuccess)
            return Result<CreateBookResponse>.FromResult(ensureRelationsExistsResult);
        
        var createBookResult = Book.Create(request.Title, request.Summary, request.Abstract, request.Price,
            request.NumberOfPages, request.Isbn, request.PublishDate, request.AuthorId, request.CategoryId);
        if (!createBookResult.IsSuccess)
            return Result<CreateBookResponse>.FromResult(createBookResult);
        
        var addCategoryResult = await bookRepository.AddAsync(createBookResult.Data!, cancellationToken);
        if (!addCategoryResult.IsSuccess)
            return Result<CreateBookResponse>.FromResult(addCategoryResult);

        return new CreateBookResponse(
            createBookResult.Data!.Title);
    }

    private async Task<Result> EnsureUniquenessAsync(CreateBookRequest request, CancellationToken cancellationToken)
    {
        var isTitleUniqueResult = await bookRepository.IsUniqueAsync(nameof(request.Title), request.Title, cancellationToken);
        if (!isTitleUniqueResult.IsSuccess)
            return isTitleUniqueResult;

        if (!isTitleUniqueResult.Data)
            return Result.WithEntityAlreadyExists(nameof(Book), request.Title,
                $"{nameof(Book)} with title {request.Title} already exists.");
        
        var isIsbnUniqueResult = await bookRepository.IsUniqueAsync(nameof(request.Isbn), request.Isbn, cancellationToken);
        if (!isIsbnUniqueResult.IsSuccess)
            return isIsbnUniqueResult;

        if (!isIsbnUniqueResult.Data)
            return Result.WithEntityAlreadyExists(nameof(Book), request.Isbn,
                $"{nameof(Book)} with ISBN {request.Isbn} already exists.");

        return Result.Success();
    }
    
    private async Task<Result> EnsureRelationsExistsAsync(CreateBookRequest request, CancellationToken cancellationToken)
    {
        var authorExistsResult = await authorRepository.ExistsAsync(request.AuthorId, cancellationToken);
        if (!authorExistsResult.IsSuccess)
            return authorExistsResult;
        
        var authorExists = authorExistsResult.Data;
        if (!authorExists)
            return Result.WithEntityNotFound(nameof(Author), request.AuthorId,
                $"{nameof(Author)} with {request.AuthorId} not found");

        var categoryExistsResult = await categoryRepository.ExistsAsync(request.CategoryId, cancellationToken);
        if (!categoryExistsResult.IsSuccess)
            return categoryExistsResult;
        
        var categoryExists = categoryExistsResult.Data;
        if (!categoryExists)
            return Result.WithEntityNotFound(nameof(Category), request.AuthorId,
                $"Category with {request.CategoryId} not found");

        return Result.Success();
    }
}