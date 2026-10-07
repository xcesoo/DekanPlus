namespace DekanPlus.Domain.Entities;

public class Specialty
{
    public Guid Id { get; private init; }
    public string StateCode { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Qualification { get; private set; } = null!;

    public Guid DirectionId { get; private set; }
    public Direction Direction { get; private set; } = null!;

    public Guid DepartmentId { get; private set; }
    public Department Department { get; private set; } = null!;

    private readonly List<Group> _groups = new();
    public IReadOnlyCollection<Group> Groups => _groups.AsReadOnly();

    private Specialty() { }

    private Specialty(string stateCode, string name, string qualification, Guid directionId, Guid departmentId)
    {
        Id = Guid.CreateVersion7();
        StateCode = stateCode;
        Name = name;
        Qualification = qualification;
        DirectionId = directionId;
        DepartmentId = departmentId;
    }

    public static Specialty Create(
        string stateCode,
        string name,
        string qualification,
        Guid directionId,
        Guid departmentId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(stateCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(qualification);
        if (directionId == Guid.Empty)
            throw new ArgumentException("Напрям підготовки не вказано.");
        if (departmentId == Guid.Empty)
            throw new ArgumentException("Випускаючу кафедру не вказано.");

        return new Specialty(stateCode.Trim(), name.Trim(), qualification.Trim(), directionId, departmentId);
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
