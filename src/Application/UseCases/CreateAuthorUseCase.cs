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
            return Result<CreateAuthorResponse>.FromResult(isUniqueResult);
        
        var emailAlreadyExists = !isUniqueResult.Data!;
        
        if (emailAlreadyExists)
            return Result<CreateAuthorResponse>.WithEntityAlreadyExists(nameof(Author), request.Email,
                $"Entity with Email {request.Email} already exists.");

        var createAuthorResult = Author.Create(request.Name, request.Email, request.Description);
        if (!createAuthorResult.IsSuccess)
            return Result<CreateAuthorResponse>.FromResult(createAuthorResult);
        
        var addAuthorResult = await authorRepository.AddAsync(createAuthorResult.Data!, cancellationToken);
        if (!addAuthorResult.IsSuccess)
            return Result<CreateAuthorResponse>.FromResult(addAuthorResult);

        return new CreateAuthorResponse(
            createAuthorResult.Data!.Name,
            createAuthorResult.Data!.Email.Address,
            createAuthorResult.Data!.Description);
    }
}