using Domain;
using Domain.Author;

namespace Application.UseCases;

public class CreateAuthorUseCase(IAuthorRepository authorRepository)
{
    public async Task<Result<CreateAuthorResponse>> HandleAsync(
        CreateAuthorRequest request, 
        CancellationToken cancellationToken)
    {
        var authorResult = Author.Create(request.Name, request.Email, request.Description);
        if (!authorResult.IsSuccess)
            return Result<CreateAuthorResponse>.FromResult<Author, CreateAuthorResponse>(authorResult);
        
        var addResult = await authorRepository.AddAsync(authorResult.Value!, cancellationToken);
        if (!addResult.IsSuccess)
            return Result<CreateAuthorResponse>.FromResult<Author, CreateAuthorResponse>(addResult);

        return new CreateAuthorResponse(
            authorResult.Value!.Id, 
            authorResult.Value!.Name,
            authorResult.Value!.Email,
            authorResult.Value!.Description, 
            addResult.Value!.CreatedAt);
    }
}