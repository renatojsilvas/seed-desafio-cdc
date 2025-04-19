using Domain.Entities;
using Infrastructure.Repositories.Dtos;

namespace Infrastructure.Repositories.Mappers;

internal static class MapperToDto
{
    public static AuthorDto ToDto(this Author author)
        => new(author.Name, 
               author.Email.Address, 
               author.Description);
    
    public static CategoryDto ToDto(this Category category)
        => new(category.Name);
    
    public static BookDto ToDto(this Book book)
        => new (
            book.Title, 
            book.Summary,
            book.Abstract,
            book.Price,
            book.NumberOfPages,
            book.Isbn,
            book.PublishDate.ToDateTime(TimeOnly.MinValue),
            book.AuthorId,
            book.CategoryId);

    public static IReadOnlyCollection<BookTitle> ToDomain(this IReadOnlyCollection<BookTitleDto> books)
        => books.Select(book => book.ToDomain()).ToList(); 
    
    private static BookTitle ToDomain(this BookTitleDto bookTitle)
        => new(bookTitle.Id, bookTitle.Title);
}