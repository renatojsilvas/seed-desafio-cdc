namespace Infrastructure.Repositories.Dtos;

public record BookTitleDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
}