using Application.Repositories.Repositories.Base;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Repositories.Repositories;

public interface IStateRepository : IRepository
{
    Task<Result<State>> AddAsync(State state, CancellationToken cancellationToken);
}