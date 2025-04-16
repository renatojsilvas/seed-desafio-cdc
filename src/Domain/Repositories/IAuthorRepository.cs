using Domain.Repositories.Base;
using Domain.ValueObjects;

namespace Domain.Repositories;

public interface IAuthorRepository : IRepository
{
    Task<Result<Entities.Author>> AddAsync(Entities.Author author, CancellationToken cancellationToken);
}