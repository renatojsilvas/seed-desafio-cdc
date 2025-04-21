namespace Application.Repositories.Repositories.Models;

public record BookDetail(
    string Title,
    string Summary,
    string Abstract,
    string Isbn,
    int NumberOfPages,
    decimal Price,
    string AuthorName,
    string AuthorDescription);
