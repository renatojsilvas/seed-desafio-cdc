namespace Infrastructure.Repositories.Dtos;

internal record AuthorDto
{
    public int AuthorId { get; init; }
    public string AuthorName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
