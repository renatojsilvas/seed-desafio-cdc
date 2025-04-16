using System.Text.RegularExpressions;
using Domain.ValueObjects;

namespace Domain.Entities;

public partial class Author
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
        List<Validation> validations = new();
        
        if (string.IsNullOrWhiteSpace(name))
            validations.Add(new Validation("Name", "Name is required"));
        
        var emailResult = Email.Create(email);
        if (!emailResult.IsSuccess)
            validations.AddRange(emailResult.Validations!);
        
        if (string.IsNullOrWhiteSpace(description))
            validations.Add(new Validation("Description", "Description is required"));
        
        if (description.Length > Constants.MaxDescriptionLength)
            validations.Add(new Validation("Description", "Description is too long. Max 400 characters"));
        
        if (validations.Any())
            return validations;
        
        return new Author(name, emailResult.Value!, description);
    }

    [GeneratedRegex(@"^(?i)[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}(?:\.[a-z]{2})?$", RegexOptions.None, "pt-BR")]
    private static partial Regex EmailRegex();
}