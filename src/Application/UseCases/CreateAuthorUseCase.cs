using Domain.Entities;
using Domain.Repositories;
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

        var authorResult = Author.Create(request.Name, request.Email, request.Description);
        if (!authorResult.IsSuccess)
            return Result<CreateAuthorResponse>.FromResult<Author, CreateAuthorResponse>(authorResult);
        
        var addResult = await authorRepository.AddAsync(authorResult.Value!, cancellationToken);
        if (!addResult.IsSuccess)
            return Result<CreateAuthorResponse>.FromResult<Author, CreateAuthorResponse>(addResult);

        return new CreateAuthorResponse(
            authorResult.Value!.Name,
            authorResult.Value!.Email.Address,
            authorResult.Value!.Description);
    }
}