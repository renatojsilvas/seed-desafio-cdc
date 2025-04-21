namespace Infrastructure.Repositories.Dtos;

internal record BookTitleDto
{
    internal int Id { get; init; }
    internal string Title { get; init; } = string.Empty;
}