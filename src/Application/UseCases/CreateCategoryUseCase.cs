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
        var isUniqueResult = await categoryRepository.IsUniqueAsync("name", request.Name, cancellationToken);
        if (!isUniqueResult.IsSuccess)
            return Result<CreateCategoryResponse>.FromResult<bool, CreateCategoryResponse>(isUniqueResult);
        
        var emailAlreadyExists = !isUniqueResult.Value!;
        
        if (emailAlreadyExists)
            return Result<CreateCategoryResponse>.FailureOnValidations([
                new Validation("Name", $"{request.Name} already exists")]);

        var createCategoryResult = Category.Create(request.Name);
        if (!createCategoryResult.IsSuccess)
            return Result<CreateCategoryResponse>.FromResult<Category, CreateCategoryResponse>(createCategoryResult);
        
        var addCategoryResult = await categoryRepository.AddAsync(createCategoryResult.Value!, cancellationToken);
        if (!addCategoryResult.IsSuccess)
            return Result<CreateAuthorResponse>.FromResult<Category, CreateCategoryResponse>(addCategoryResult);

        return new CreateCategoryResponse(
            createCategoryResult.Value!.Name);
    }
}