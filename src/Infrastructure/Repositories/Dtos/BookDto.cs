namespace Infrastructure.Repositories.Dtos;

internal record BookDto(
    string Title,
    string Summary,
    string Abstract,
    decimal Price,
    uint NumberOfPages,
    string Isbn,
    DateTime PublishDate, 
    int AuthorId,
    int CategoryId);
