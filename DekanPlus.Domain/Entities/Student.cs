using DekanPlus.Domain.Enums;
using DekanPlus.Domain.ValueObjects;

namespace DekanPlus.Domain.Entities;

public class Student
{
    public Guid Id { get; private init; }

    public FullName FullName { get; private set; } = null!;
    public DateOnly BirthDate { get; private set; }
    public string HomeAddress { get; private set; } = null!;
    public string? ResidenceAddress { get; private set; }

    public int EnrollmentYear { get; private set; }
    public StudyForm StudyForm { get; private set; }
    public string RecordBookNumber { get; private set; } = null!;

    public Expulsion? Expulsion { get; private set; }
    public bool IsExpelled => Expulsion is not null;

    public Guid GroupId { get; private set; }
    public Group Group { get; private set; } = null!;

    private Student() { }

    private Student(
        FullName fullName,
        DateOnly birthDate,
        string homeAddress,
        string? residenceAddress,
        int enrollmentYear,
        StudyForm studyForm,
        string recordBookNumber,
        Guid groupId)
    {
        Id = Guid.CreateVersion7();
        FullName = fullName;
        BirthDate = birthDate;
        HomeAddress = homeAddress;
        ResidenceAddress = residenceAddress;
        EnrollmentYear = enrollmentYear;
        StudyForm = studyForm;
        RecordBookNumber = recordBookNumber;
        GroupId = groupId;
    }

    public static Student Create(
        FullName fullName,
        DateOnly birthDate,
        string homeAddress,
        string? residenceAddress,
        int enrollmentYear,
        StudyForm studyForm,
        string recordBookNumber,
        Guid groupId)
    {
        ArgumentNullException.ThrowIfNull(fullName);
        ArgumentException.ThrowIfNullOrWhiteSpace(homeAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(recordBookNumber);
        if (birthDate == default)
            throw new ArgumentException("Дата народження не може бути порожньою.");
        if (enrollmentYear <= 0)
            throw new ArgumentException("Рік вступу має бути більшим за нуль.");
        EnsureValidStudyForm(studyForm);
        if (groupId == Guid.Empty)
            throw new ArgumentException("Групу не вказано.");

        return new Student(
            fullName,
            birthDate,
            homeAddress.Trim(),
            NormalizeOptional(residenceAddress),
            enrollmentYear,
            studyForm,
            recordBookNumber.Trim(),
            groupId);
    }

    public void ChangeFullName(FullName newFullName)
    {
        EnsureNotExpelled();
        ArgumentNullException.ThrowIfNull(newFullName);
        FullName = newFullName;
    }

    public void ChangeAddresses(string newHomeAddress, string? newResidenceAddress)
    {
        EnsureNotExpelled();
        ArgumentException.ThrowIfNullOrWhiteSpace(newHomeAddress);
        HomeAddress = newHomeAddress.Trim();
        ResidenceAddress = NormalizeOptional(newResidenceAddress);
    }

    public void ChangeStudyForm(StudyForm newStudyForm)
    {
        EnsureNotExpelled();
        EnsureValidStudyForm(newStudyForm);
        StudyForm = newStudyForm;
    }

    public void TransferToGroup(Guid newGroupId)
    {
        EnsureNotExpelled();
        if (newGroupId == Guid.Empty)
            throw new ArgumentException("Групу не вказано.");
        GroupId = newGroupId;
    }

    public void Expel(DateOnly date, ExpulsionReason reason)
    {
        EnsureNotExpelled();
        Expulsion = Expulsion.Create(date, reason);
    }

    public void Reinstate()
    {
        if (!IsExpelled)
            throw new InvalidOperationException("Студента не відраховано.");
        Expulsion = null;
    }

    private void EnsureNotExpelled()
    {
        if (IsExpelled)
            throw new InvalidOperationException("Студента вже відраховано.");
    }

    private static void EnsureValidStudyForm(StudyForm studyForm)
    {
        if (studyForm == StudyForm.Unknown || !Enum.IsDefined(studyForm))
            throw new ArgumentException("Невідомий вид навчання.");
    }

    private static string? NormalizeOptional(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
