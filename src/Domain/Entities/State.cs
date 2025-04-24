using Domain.ValueObjects;

namespace Domain.Entities;

public class State
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    
    public int CountryId { get; private set; }

    private State(int id, string name, int countryId)
    {
        Id = id;
        Name = name;
        CountryId = countryId;
    }
    
    public static Result<State> Create(string name, int countryId) => new State(0, name, countryId);
}