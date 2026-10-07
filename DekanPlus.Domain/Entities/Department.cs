namespace DekanPlus.Domain.Entities;

public class Department
{
    public Guid Id { get; private init; }
    public string Name { get; private set; } = null!;

    private readonly List<Specialty> _specialties = new();
    public IReadOnlyCollection<Specialty> Specialties => _specialties.AsReadOnly();

    private Department() { }

    private Department(string name)
    {
        Id = Guid.CreateVersion7();
        Name = name;
    }

    public static Department Create(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new Department(name.Trim());
    }

    public void ChangeName(string newName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);
        Name = newName.Trim();
    }
}
