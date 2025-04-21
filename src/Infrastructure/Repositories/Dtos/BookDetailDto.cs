namespace Infrastructure.Repositories.Dtos;

internal record BookDetailDto
{
    internal string Title { get; init; } = string.Empty;
    internal AuthorDto Author { get; set; } = null!;
    internal CategoryDto Category { get; set; } = null!;
    internal string Summary { get; init; } = string.Empty;
    internal string Abstract { get; init; } = string.Empty;
    internal decimal Price { get; init; }
    internal int NumberOfPages { get; init; }
    internal string Isbn { get; init; } = string.Empty;
    internal DateTime PublishDate { get; init; }
}