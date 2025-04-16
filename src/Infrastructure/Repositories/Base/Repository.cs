using System.Data;
using Dapper;
using Domain.Repositories.Base;
using Domain.ValueObjects;

namespace Infrastructure.Repositories.Base;

public abstract class Repository(
    IDbConnection dbConnection, 
    string tableName) : IRepository
{
    public Task<Result<bool>> IsUniqueAsync(string column, object value, CancellationToken cancellationToken)
        => IsUniqueAsync(tableName, column, value, cancellationToken);

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
}