using Application.Repositories.Repositories.Base;
using Domain.ValueObjects;

namespace Application.Repositories.Repositories;

public interface ICategoryRepository : IRepository
{
    Task<Result<Domain.Entities.Category>> AddAsync(Domain.Entities.Category author, CancellationToken cancellationToken);
}