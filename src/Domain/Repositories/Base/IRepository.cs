using Domain.ValueObjects;

namespace Domain.Repositories.Base;

public interface IRepository
{   
    Task<Result<bool>> IsUniqueAsync(string column, object value, CancellationToken cancellationToken);
}