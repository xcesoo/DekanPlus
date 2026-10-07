namespace DekanPlus.Domain.ValueObjects;

public sealed record FullName
{
    public string LastName { get; private init; } = null!;
    public string FirstName { get; private init; } = null!;
    public string? MiddleName { get; private init; }

    private FullName() { }

    private FullName(string lastName, string firstName, string? middleName)
    {
        LastName = lastName;
        FirstName = firstName;
        MiddleName = middleName;
    }

    public static FullName Create(string lastName, string firstName, string? middleName = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);

        return new FullName(
            lastName.Trim(),
            firstName.Trim(),
            string.IsNullOrWhiteSpace(middleName) ? null : middleName.Trim());
    }
}
