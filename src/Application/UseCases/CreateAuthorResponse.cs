namespace Application.UseCases;

public record CreateAuthorResponse(Guid Id, string Name, string Email, string Description, DateTime CreatedAt);