using Application.Repositories.Repositories;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.UseCases;

public class CreateStateUseCase(
    IStateRepository stateRepository,
    ICountryRepository countryRepository)
{
    public record CreateStateRequest(string Name, int CountryId);
    public record CreateStateResponse(string Name);

    public async Task<Result<CreateStateResponse>> HandleAsync(CreateStateRequest request, 
        CancellationToken cancellationToken)
    {
        var isUniqueResult = await stateRepository.IsUniqueAsync("name", request.Name, cancellationToken);
        if (!isUniqueResult.IsSuccess)
            return Result<CreateStateResponse>.FromResult(isUniqueResult);
        
        var nameAlreadyExists = !isUniqueResult.Data!;
        
        if (nameAlreadyExists)
            return Result<CreateStateResponse>.WithEntityAlreadyExists(nameof(Country), request.Name,
                $"{nameof(State)} with Name {request.Name} already exists.");
        
        var isCountryExistsResult = await countryRepository.ExistsAsync(request.CountryId, cancellationToken);
        if (!isCountryExistsResult.IsSuccess)
            return Result<CreateStateResponse>.FromResult(isCountryExistsResult);
        
        var countryExists = isCountryExistsResult.Data;
        if (!countryExists)
            return Result<CreateStateResponse>.WithEntityNotFound(nameof(Country), request.CountryId,
                $"{nameof(Country)} with {request.CountryId} does not exists.");

        var stateResult = State.Create(request.Name, request.CountryId);
        if (!stateResult.IsSuccess)
            return Result<CreateStateResponse>.FromResult(stateResult);
        
        var addStateResult = await stateRepository.AddAsync(stateResult.Data!, cancellationToken);
        if (!addStateResult.IsSuccess)
            return Result<CreateStateResponse>.FromResult(addStateResult);
        
        return new CreateStateResponse(stateResult.Data!.Name);
    }
}