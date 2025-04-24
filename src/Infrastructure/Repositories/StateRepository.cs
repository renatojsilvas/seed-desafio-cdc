using System.Data;
using Application.Repositories.Repositories;
using Dapper;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Repositories.Base;
using Infrastructure.Repositories.Mappers;

namespace Infrastructure.Repositories;

public class StateRepository(IDbConnection dbConnection)
    : Repository(dbConnection, "states"), IStateRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<Result<State>> AddAsync(State state, CancellationToken cancellationToken)
    {
        try
        {
            var stateDto = state.ToDto();

            var parameters = new
            {
                Name = stateDto.StateName,
                CountryId = stateDto.CountryId,
            };

            var result = await _dbConnection.ExecuteAsync(
                new CommandDefinition(InsertStateSql, parameters, cancellationToken: cancellationToken));

            return result > 0 ? state : Result<State>.WithError("Fail to insert state");
        }
        catch (Exception e)
        {
            return e;
        }
    }

    private const string InsertStateSql =
        """
            INSERT INTO cdc.states (name, country_id)
            VALUES (@name, @countryId)
        """;
}