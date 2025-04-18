using Application.Repositories.Repositories.Base;
using Domain.ValueObjects;

namespace Application.Repositories.Repositories;

public interface IAuthorRepository : IRepository
{
    Task<Result<Domain.Entities.Author>> AddAsync(Domain.Entities.Author author, CancellationToken cancellationToken);
}