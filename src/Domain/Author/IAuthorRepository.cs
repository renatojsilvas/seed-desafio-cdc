namespace Domain.Author;

public interface IAuthorRepository
{
    Task<Result<Author>> AddAsync(Author author, CancellationToken cancellationToken);
}