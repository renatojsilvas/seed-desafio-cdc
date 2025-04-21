using Domain.ValueObjects;

namespace Domain.Entities;

public class Category
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    
    private Category (string name)
    {
        Name = name;
    }

    public static Result<Category> Create(string name) => new Category(name);
}