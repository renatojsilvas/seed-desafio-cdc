using Application.Repositories.Repositories.Models;
using Domain.Entities;
using Infrastructure.Repositories.Dtos;

namespace Infrastructure.Repositories.Mappers;

internal static class MapperToDto
{
    public static AuthorDto ToDto(this Author author)
        => new AuthorDto()
        {
            AuthorName = author.Name,
            Email = author.Email.Address,
            Description = author.Description
        };

    public static CategoryDto ToDto(this Category category)
        => new CategoryDto()
        {
            CategoryId = category.Id,
            CategoryName = category.Name,
        };
    
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

    public static CountryDto ToDto(this Country country)
        => new()
        {
            CountryName = country.Name
        };
    
    public static StateDto ToDto(this State state)
        => new()
        {
            StateName = state.Name,
            CountryId = state.CountryId
        };

    public static IReadOnlyCollection<BookTitle> ToDomain(this IReadOnlyCollection<BookTitleDto> books)
        => books.Select(book => book.ToDomain()).ToList(); 
    
    private static BookTitle ToDomain(this BookTitleDto bookTitle)
        => new(bookTitle.Id, bookTitle.Title);

    public static BookDetail ToDomain(this BookDetailDto bookDetail)
    {
        return new BookDetail(
            Title: bookDetail.Title,
            Summary: bookDetail.Summary,
            Abstract: bookDetail.Abstract,
            Price: bookDetail.Price,
            NumberOfPages: bookDetail.NumberOfPages,
            Isbn: bookDetail.Isbn,
            AuthorName: bookDetail.Author.AuthorName,
            AuthorDescription: bookDetail.Author.Description);
    }
}