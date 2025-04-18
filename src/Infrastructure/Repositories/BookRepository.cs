using System.Data;
using Application.Repositories.Repositories;
using Dapper;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Repositories.Base;
using Infrastructure.Repositories.Mappers;

namespace Infrastructure.Repositories;

public class BookRepository(IDbConnection dbConnection)
    : Repository(dbConnection, "books"), IBookRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;
    
    public async Task<Result<Book>> AddAsync(Book book, CancellationToken cancellationToken)
    {
        try
        {
            var authorDto = book.ToDto();
        
            var parameters = new
            {
                Title = authorDto.Title,
                Summary = authorDto.Summary,
                Abstract = authorDto.Abstract,
                Price = authorDto.Price,
                Isbn = authorDto.Isbn,
                NumberOfPages = authorDto.NumberOfPages,
                PublishDate = authorDto.PublishDate,
                AuthorId = authorDto.AuthorId,
                CategoryId = authorDto.CategoryId,
            };

            var result = await _dbConnection.ExecuteAsync(
                new CommandDefinition(InsertBookSql, parameters, cancellationToken: cancellationToken));
        
            return result > 0 ? book : Result<Book>.Failure("Fail to insert book");
        }
        catch (Exception e)
        {
            return e;
        }
    }
    
    private const string InsertBookSql = 
        $"""
         INSERT INTO cdc.books (title, summary, abstract, price, isbn, number_of_pages, publish_date, author_id, category_id) 
         VALUES (@title, @summary, @abstract, @price, @isbn, @numberOfPages, @publishDate, @authorId, @categoryId)
         """;
}