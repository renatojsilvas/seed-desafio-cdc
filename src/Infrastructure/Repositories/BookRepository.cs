using System.Data;
using Application.Repositories.Repositories;
using Application.Repositories.Repositories.Models;
using Dapper;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Repositories.Base;
using Infrastructure.Repositories.Dtos;
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
        
            return result > 0 ? book : Result<Book>.WithError("Fail to insert book");
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<BookDetail>> GetDetailAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            BookDetailDto? bookDetailDto = null;
           _ = await _dbConnection.QueryAsync<BookDetailDto, AuthorDto, CategoryDto, BookDetailDto>(
                new CommandDefinition(GetBookDetailSql, new { Id = id }, cancellationToken: cancellationToken),
                (book, author, category) =>
                {
                    bookDetailDto = book;
                    bookDetailDto.Author = author;
                    bookDetailDto.Category = category;
                    return bookDetailDto;
                },
                splitOn: "AuthorId, CategoryId");

           return bookDetailDto?.ToDomain() ?? 
                  Result<BookDetail>.WithEntityNotFound(nameof(Book), id, 
                      $"{nameof(Book)} with {id} not found");
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<IReadOnlyCollection<BookTitle>>> ListTitlesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var bookTitles = (await _dbConnection.QueryAsync<BookTitleDto>(
                new CommandDefinition(ListBookTitlesSql, cancellationToken: cancellationToken))).ToList();
            
            return bookTitles.Count == 0 ?
                Result<IReadOnlyCollection<BookTitle>>.WithNoContent() :
                bookTitles.ToDomain().ToList();
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
    
    private const string ListBookTitlesSql = 
        $"""
         SELECT 
             b.id As Id,
             b.title As Title
         FROM cdc.books b
         """;
    
    private const string GetBookDetailSql = 
        $"""
         SELECT 
         	b.title AS Title,
         	b.summary AS Summary,
         	b.abstract AS Abstract,
         	b.number_of_pages  As NumberOfPages,
         	b.price AS Price,
         	b.isbn AS Isbn,
         	b.publish_date AS PublishDate,
         	a.id As AuthorId,
         	a.name As AuthorName,
         	a.email As Email,
         	a.description As Description,
         	c.id As CategoryId,
         	c.name AS CategoryName
         FROM cdc.books b 
         JOIN cdc.authors a ON a.id = b.author_id
         JOIN cdc.categories c ON c.id = b.category_id
         WHERE b.id = @Id
         """;
}