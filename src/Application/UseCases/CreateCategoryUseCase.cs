using Application.Repositories.Repositories;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.UseCases;

public record CreateCategoryRequest(string Name);
public record CreateCategoryResponse(string Name);

public class CreateCategoryUseCase(ICategoryRepository categoryRepository)
{
    public async Task<Result<CreateCategoryResponse>> HandleAsync(
        CreateCategoryRequest request, 
        CancellationToken cancellationToken)
    {
        var isUniqueResult = await categoryRepository.IsUniqueAsync(nameof(request.Name), request.Name, cancellationToken);
        if (!isUniqueResult.IsSuccess)
            return Result<CreateCategoryResponse>.FromResult(isUniqueResult);
        
        var emailAlreadyExists = !isUniqueResult.Data!;
        
        if (emailAlreadyExists)
            return Result<CreateCategoryResponse>.WithEntityAlreadyExists(nameof(Category), request.Name,
                $"Entity with Name {request.Name} already exists.");

        var createCategoryResult = Category.Create(request.Name);
        if (!createCategoryResult.IsSuccess)
            return Result<CreateCategoryResponse>.FromResult(createCategoryResult);
        
        var addCategoryResult = await categoryRepository.AddAsync(createCategoryResult.Data!, cancellationToken);
        if (!addCategoryResult.IsSuccess)
            return Result<CreateCategoryResponse>.FromResult(addCategoryResult);

        return new CreateCategoryResponse(
            createCategoryResult.Data!.Name);
    }
}