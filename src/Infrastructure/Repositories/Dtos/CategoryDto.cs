namespace Infrastructure.Repositories.Dtos;

internal record CategoryDto
{
    public int CategoryId { get; init; }
    public string CategoryName { get; init; }  = string.Empty;
}