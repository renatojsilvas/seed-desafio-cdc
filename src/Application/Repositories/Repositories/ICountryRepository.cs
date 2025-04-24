using Application.Repositories.Repositories.Base;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Repositories.Repositories;

public interface ICountryRepository : IRepository
{
    Task<Result<Country>> AddAsync(Country country, CancellationToken cancellationToken);
}