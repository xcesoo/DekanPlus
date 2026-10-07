namespace DekanPlus.Domain.Entities;

public class Direction
{
    public Guid Id { get; private init; }
    public string StateCode { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Qualification { get; private set; } = null!;

    private readonly List<Specialty> _specialties = new();
    public IReadOnlyCollection<Specialty> Specialties => _specialties.AsReadOnly();

    private Direction() { }

    private Direction(string stateCode, string name, string qualification)
    {
        Id = Guid.CreateVersion7();
        StateCode = stateCode;
        Name = name;
        Qualification = qualification;
    }

    public static Direction Create(string stateCode, string name, string qualification)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(stateCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(qualification);
        return new Direction(stateCode.Trim(), name.Trim(), qualification.Trim());
    }

    public void ChangeStateCode(string newStateCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newStateCode);
        StateCode = newStateCode.Trim();
    }

    public void ChangeName(string newName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);
        Name = newName.Trim();
    }

    public void ChangeQualification(string newQualification)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newQualification);
        Qualification = newQualification.Trim();
    }
}
