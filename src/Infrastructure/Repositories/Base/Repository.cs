using System.Data;
using Application.Repositories.Repositories.Base;
using Dapper;
using Domain.ValueObjects;

namespace Infrastructure.Repositories.Base;

public abstract class Repository(
    IDbConnection dbConnection, 
    string tableName) : IRepository
{
    public Task<Result<bool>> IsUniqueAsync(string property, object value, CancellationToken cancellationToken)
        => IsUniqueAsync(tableName, property, value, cancellationToken);
    
    public Task<Result<bool>> ExistsAsync(int id, CancellationToken cancellationToken)
        => ExistsAsync(tableName, id, cancellationToken);

    private async Task<Result<bool>> IsUniqueAsync(string table, string column, object value,
        CancellationToken cancellationToken)
    {
        try
        {
            var sql = $@"
            SELECT COUNT(*) 
            FROM cdc.`{table}`
            WHERE `{column}` = @Value
            LIMIT 1";

            var count = await dbConnection.ExecuteScalarAsync<int>(sql, new { Value = value });
            return count == 0;
        }
        catch (Exception e)
        {
            return e;
        }
    }
    
    private async Task<Result<bool>> ExistsAsync(string table, int id, CancellationToken cancellationToken)
    {
        try
        {
            var sql = $@"
            SELECT COUNT(*) FROM cdc.`{table}` 
            WHERE id = @Id
            LIMIT 1";

            var count = await dbConnection.ExecuteScalarAsync<int>(sql, new { Id = id });
            return count > 0;
        }
        catch (Exception e)
        {
            return e;
        }
    }
}