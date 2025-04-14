using Domain.Author;
using Infrastructure.Repositories.Dtos;

namespace Infrastructure.Repositories.Mappers;

public static class MapperToDto
{
    public static AuthorDto ToDto(this Author author)
        => new(author.Name, 
               author.Email, 
               author.Description);
}