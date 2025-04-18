using Domain.ValueObjects;

namespace Application.Repositories.Repositories.Base;

public interface IRepository
{   
    Task<Result<bool>> IsUniqueAsync(string property, object value, CancellationToken cancellationToken);
    Task<Result<bool>> ExistsAsync(int id, CancellationToken cancellationToken);
}