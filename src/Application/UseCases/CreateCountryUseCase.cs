using Application.Repositories.Repositories;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.UseCases;

public class CreateCountryUseCase(ICountryRepository countryRepository)
{
    public record CreateCountryRequest(string Name);
    public record CreateCountryResponse(string Name);

    public async Task<Result<CreateCountryResponse>> HandleAsync(CreateCountryRequest request, 
        CancellationToken cancellationToken)
    {
        var isUniqueResult = await countryRepository.IsUniqueAsync("name", request.Name, cancellationToken);
        if (!isUniqueResult.IsSuccess)
            return Result<CreateCountryResponse>.FromResult(isUniqueResult);
        
        var nameAlreadyExists = !isUniqueResult.Data!;
        
        if (nameAlreadyExists)
            return Result<CreateCountryResponse>.WithEntityAlreadyExists(nameof(Country), request.Name,
                $"{nameof(Country)} with Name {request.Name} already exists.");

        var countryResult = Country.Create(request.Name);
        if (!countryResult.IsSuccess)
            return Result<CreateCountryResponse>.FromResult(countryResult);
        
        var addCountryResult = await countryRepository.AddAsync(countryResult.Data!, cancellationToken);
        if (!addCountryResult.IsSuccess)
            return Result<CreateCountryResponse>.FromResult(addCountryResult);
        
        return new CreateCountryResponse(countryResult.Data!.Name);
    }
}