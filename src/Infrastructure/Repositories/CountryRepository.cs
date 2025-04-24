using System.Data;
using Application.Repositories.Repositories;
using Dapper;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Repositories.Base;
using Infrastructure.Repositories.Mappers;

namespace Infrastructure.Repositories;

public class CountryRepository(IDbConnection dbConnection)
    : Repository(dbConnection, "countries"), ICountryRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<Result<Country>> AddAsync(Country country, CancellationToken cancellationToken)
    {
        try
        {
            var countryDto = country.ToDto();

            var parameters = new
            {
                Name = countryDto.CountryName,
            };

            var result = await _dbConnection.ExecuteAsync(
                new CommandDefinition(InsertCountrySql, parameters, cancellationToken: cancellationToken));

            return result > 0 ? country : Result<Country>.WithError("Fail to insert country");
        }
        catch (Exception e)
        {
            return e;
        }
    }

    private const string InsertCountrySql =
        """
            INSERT INTO cdc.countries (name)
            VALUES (@name)
        """;
}