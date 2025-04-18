using Domain.ValueObjects;

namespace Application.Repositories.Repositories.Base;

public interface IRepository
{   
    Task<Result<bool>> IsUniqueAsync(string property, object value, CancellationToken cancellationToken);
}