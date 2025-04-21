using Application.UseCases;
using FastEndpoints;

namespace WebApi.Endpoints.ListBookTitles;

public class ListBookTitlesValidator : Validator<ListBookTitlesUseCase.ListBookTitlesRequest>
{
    public ListBookTitlesValidator()
    {
    }
}