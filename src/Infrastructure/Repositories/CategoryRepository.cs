using System.Data;
using Application.Repositories.Repositories;
using Dapper;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Repositories.Base;
using Infrastructure.Repositories.Mappers;

namespace Infrastructure.Repositories;

internal sealed class CategoryRepository(IDbConnection dbConnection)
    : Repository(dbConnection, "categories"), ICategoryRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;
    
    public async Task<Result<Category>> AddAsync(Category category, CancellationToken cancellationToken)
    {
        try
        {
            var authorDto = category.ToDto();
        
            var parameters = new
            {
                Name = authorDto.Name
            };

            var result = await _dbConnection.ExecuteAsync(
                new CommandDefinition(InsertCategorySql, parameters, cancellationToken: cancellationToken));
        
            return result > 0 ? category : Result<Category>.Failure("Fail to insert category");
        }
        catch (Exception e)
        {
            return e;
        }
    }
    
    private const string InsertCategorySql = 
        $"""
         INSERT INTO cdc.categories (name) 
         VALUES (@name)
         """;
}