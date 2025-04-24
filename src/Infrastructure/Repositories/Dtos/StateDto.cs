namespace Infrastructure.Repositories.Dtos;

public record StateDto
{
    public string StateName { get; init; } = string.Empty;
    public int CountryId { get; init; }
}