using Application.UseCases;
using Domain;
using FastEndpoints;
using FluentValidation;

namespace WebApi.Endpoints.CreateCategory;

public class ListBookTitlesValidator : Validator<ListBookTitlesUseCase.ListBookTitlesRequest>
{
    public ListBookTitlesValidator()
    {
    }
}