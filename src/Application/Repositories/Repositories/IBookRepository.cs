using Application.Repositories.Repositories.Base;
using Domain.ValueObjects;

namespace Application.Repositories.Repositories;

public interface IBookRepository : IRepository
{
    Task<Result<Domain.Entities.Book>> AddAsync(Domain.Entities.Book book, CancellationToken cancellationToken);
}