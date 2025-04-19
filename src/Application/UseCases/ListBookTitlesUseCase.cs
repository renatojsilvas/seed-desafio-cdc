using Application.Repositories.Repositories;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.UseCases;

public class ListBookTitlesUseCase(IBookRepository bookRepository)
{
    public record ListBookTitlesRequest(int Page = 1, int PageSize = 10);
    public record ListBookTitlesResponse(IReadOnlyCollection<BookTitle> BookTitles);

    public async Task<Result<ListBookTitlesResponse>> HandleAsync(ListBookTitlesRequest request,
        CancellationToken cancellationToken)
    {
        var listTitlesResult = await bookRepository.ListTitlesAsync(cancellationToken);
        if (!listTitlesResult.IsSuccess)
            return Result<ListBookTitlesResponse>
                       .FromResult<IReadOnlyCollection<BookTitle>, ListBookTitlesResponse> 
                    (listTitlesResult);
        
        return new ListBookTitlesResponse(listTitlesResult.Value!.ToList());
    }
}