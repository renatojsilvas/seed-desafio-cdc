using System.Text.RegularExpressions;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Author
{
    public uint Id { get; private set; }
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public string Description { get; private set; }

    private Author(string name, Email email, string description)
    {
        Name = name;
        Email = email;
        Description = description;
    }

    public static Result<Author> Create(string name, string email, string description)
    {
        List<Validation> validations = [];
        
        var emailResult = Email.Create(email);
        if (!emailResult.IsSuccess)
            validations.AddRange(emailResult.Validations!);
        
        if (validations.Any())
            return validations;
        
        return new Author(name, emailResult.Value!, description);
    }
}