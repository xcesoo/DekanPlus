namespace DekanPlus.Domain.Entities;

public class Group
{
    public Guid Id { get; private init; }
    public string Name { get; private set; } = null!;

    public Guid SpecialtyId { get; private set; }
    public Specialty Specialty { get; private set; } = null!;

    private readonly List<Student> _students = new();
    public IReadOnlyCollection<Student> Students => _students.AsReadOnly();

    private Group() { }

    private Group(string name, Guid specialtyId)
    {
        Id = Guid.CreateVersion7();
        Name = name;
        SpecialtyId = specialtyId;
    }

    public static Group Create(string name, Guid specialtyId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (specialtyId == Guid.Empty)
            throw new ArgumentException("Фах не вказано.");

        return new Group(name.Trim(), specialtyId);
    }

    public void ChangeName(string newName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);
        Name = newName.Trim();
    }
}
