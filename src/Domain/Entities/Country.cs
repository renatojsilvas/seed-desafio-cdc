using Domain.ValueObjects;

namespace Domain.Entities;

public class Country
{
    public int Id { get; private set; }
    public string Name { get; private set; }

    private Country(int id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public static Result<Country> Create(string name) => new Country(0, name);
}