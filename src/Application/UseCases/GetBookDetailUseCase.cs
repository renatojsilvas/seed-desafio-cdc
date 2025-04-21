using Application.Repositories.Repositories;
using Application.Repositories.Repositories.Models;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.UseCases;

public class GetBookDetailUseCase(IBookRepository bookRepository)
{
    public record GetBookDetailsRequest(int Id);
    public record GetBookDetailResponse(BookDetail Book);

    public async Task<Result<GetBookDetailResponse>> HandleAsync(GetBookDetailsRequest request,
        CancellationToken cancellationToken)
    {
        var getResult = await bookRepository.GetDetailAsync(request.Id, cancellationToken);
        if (!getResult.IsSuccess)
            return Result<GetBookDetailResponse>.FromResult(getResult);
        
        return new GetBookDetailResponse(getResult.Data!);
    }
}