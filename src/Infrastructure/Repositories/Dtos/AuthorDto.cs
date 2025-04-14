using Domain.Author;

namespace Infrastructure.Repositories.Dtos;

public record AuthorDto(string Name, string Email, string Description);
