using Application.Repositories.Repositories.Base;
using Application.Repositories.Repositories.Models;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Repositories.Repositories;

public interface IBookRepository : IRepository
{
    Task<Result<Book>> AddAsync(Book book, CancellationToken cancellationToken);
    Task<Result<BookDetail>> GetDetailAsync(int id, CancellationToken cancellationToken);
    Task<Result<IReadOnlyCollection<BookTitle>>> ListTitlesAsync(CancellationToken cancellationToken);
}