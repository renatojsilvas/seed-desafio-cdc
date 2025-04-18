using Application.Repositories.Repositories;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.UseCases;

public record CreateAuthorRequest(string Name, string Email, string Description);
public record CreateAuthorResponse(string Name, string Email, string Description);

public class CreateAuthorUseCase(IAuthorRepository authorRepository)
{
    public async Task<Result<CreateAuthorResponse>> HandleAsync(
        CreateAuthorRequest request, 
        CancellationToken cancellationToken)
    {
        var isUniqueResult = await authorRepository.IsUniqueAsync("email", request.Email, cancellationToken);
        if (!isUniqueResult.IsSuccess)
            return Result<CreateAuthorResponse>.FromResult<bool, CreateAuthorResponse>(isUniqueResult);
        
        var emailAlreadyExists = !isUniqueResult.Value!;
        
        if (emailAlreadyExists)
            return Result<CreateAuthorResponse>.FailureOnValidations([
                new Validation("Email", $"{request.Email} already exists")]);

        var createAuthorResult = Author.Create(request.Name, request.Email, request.Description);
        if (!createAuthorResult.IsSuccess)
            return Result<CreateAuthorResponse>.FromResult<Author, CreateAuthorResponse>(createAuthorResult);
        
        var addAuthorResult = await authorRepository.AddAsync(createAuthorResult.Value!, cancellationToken);
        if (!addAuthorResult.IsSuccess)
            return Result<CreateAuthorResponse>.FromResult<Author, CreateAuthorResponse>(addAuthorResult);

        return new CreateAuthorResponse(
            createAuthorResult.Value!.Name,
            createAuthorResult.Value!.Email.Address,
            createAuthorResult.Value!.Description);
    }
}