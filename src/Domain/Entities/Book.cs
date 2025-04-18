using Domain.ValueObjects;
using static System.String;

namespace Domain.Entities;

public class Book
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Summary { get; private set; }
    public string Abstract { get; private set; }
    public decimal Price { get; private set; }
    public uint NumberOfPages { get; private set; }
    public string Isbn { get; private set; }
    public DateOnly PublishDate { get; private set; }
    public int AuthorId { get; private set; }
    public int CategoryId { get; private set; }

    private Book(
        int id, 
        string title, 
        string summary, 
        string @abstract,
        decimal price, 
        uint numberOfPages, 
        string isbn, 
        DateOnly publishDate, 
        int authorId, 
        int categoryId)
    {
        Id = id;
        Title = title;
        Summary = summary;
        Abstract = @abstract;
        Price = price;
        NumberOfPages = numberOfPages;
        Isbn = isbn;
        PublishDate = publishDate;
        AuthorId = authorId;
        CategoryId = categoryId;
    }
    
    public static Result<Book> Create(
        string title, 
        string summary, 
        string @abstract,
        decimal price, 
        uint numberOfPages, 
        string isbn, 
        DateOnly publishDate, 
        int authorId, 
        int categoryId) =>
        new Book(0, title, summary, @abstract, price, numberOfPages, isbn, publishDate, authorId, categoryId);

    public static Book Create(
        int id, 
        string title, 
        string summary, 
        string @abstract,
        decimal price, 
        uint numberOfPages, 
        string isbn, 
        DateOnly publishDate, 
        int authorId, 
        int categoryId) =>
        new(id, title, summary, @abstract, price, numberOfPages, isbn, publishDate, authorId, categoryId);
}