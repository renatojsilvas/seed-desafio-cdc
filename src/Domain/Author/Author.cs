namespace Domain.Author;

public class Author
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    private Author()
    {
    }

    private Author(Guid id, DateTime createdAt, string name, string email, string description)
    {
        Id = id;
        CreatedAt = createdAt;
        Name = name;
        Email = email;
        Description = description;
    }

    public static Result<Author> Create(string name, string email, string description)
    {
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        return new Author(id, createdAt, name, email, description);
    }
}