using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public sealed partial record Email
{
    public string Address { get; }

    private Email(string address)
    {
        Address = address;
    }

    public static Result<Email> Create(string address)
    {
        var validations = new List<Validation>();
        
        if (string.IsNullOrWhiteSpace(address))
            validations.Add(new Validation(nameof(Address), "Email address is required."));

        if (!EmailRegex().IsMatch(address))
            validations.Add(new Validation(nameof(Address), "Email is invalid."));

        if (validations.Any())
            return validations;
        
        return new Email(address);
    }

    [GeneratedRegex(@"^(?i)[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}(?:\.[a-z]{2})?$")]
    private static partial Regex EmailRegex();
}