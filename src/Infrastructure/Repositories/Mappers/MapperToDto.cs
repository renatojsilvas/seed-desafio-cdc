using Domain.Entities;
using Infrastructure.Repositories.Dtos;

namespace Infrastructure.Repositories.Mappers;

internal static class MapperToDto
{
    public static AuthorDto ToDto(this Author author)
        => new(author.Name, 
               author.Email.Address, 
               author.Description);
    
    public static CategoryDto ToDto(this Category author)
        => new(author.Name);
}