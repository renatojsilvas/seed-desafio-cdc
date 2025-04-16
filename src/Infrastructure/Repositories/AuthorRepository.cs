using System.Data;
using Dapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Infrastructure.Repositories.Base;
using Infrastructure.Repositories.Mappers;

namespace Infrastructure.Repositories;

public class AuthorRepository(IDbConnection dbConnection) 
    : Repository(dbConnection, "authors"), IAuthorRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<Result<Author>> AddAsync(Author author, CancellationToken cancellationToken)
    {
        try
        {
            var authorDto = author.ToDto();
        
            var parameters = new
            {
                Name = authorDto.Name,
                Email = authorDto.Email,
                Description = authorDto.Description,
            };

            var result = await _dbConnection.ExecuteAsync(
                new CommandDefinition(InsertAuthorSql, parameters, cancellationToken: cancellationToken));
        
            return result > 0 ? author : Result<Author>.Failure("Fail to insert author");
        }
        catch (Exception e)
        {
            return e;
        }
    }
    
    private const string InsertAuthorSql = 
        $"""
         INSERT INTO cdc.authors (name, email, description) 
         VALUES (@name, @email, @description)
         """;
}