using System.Reflection;
using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Enums;
using DekanPlus.Domain.ValueObjects;

namespace DekanPlus.Tests.TestHelpers;

internal static class TestFactory
{
    public static Student CreateStudent(
        int enrollmentYear = 2024,
        StudyForm studyForm = StudyForm.FullTimeContract,
        Guid? groupId = null) =>
        Student.Create(
            FullName.Create("Іваненко", "Іван", "Іванович"),
            new DateOnly(2006, 5, 15),
            "м. Київ, вул. Хрещатик, 1",
            null,
            enrollmentYear,
            studyForm,
            "F3-24-00001",
            groupId ?? Guid.CreateVersion7());

    public static Student CreateExpelledStudent(
        DateOnly expulsionDate,
        ExpulsionReason reason,
        int enrollmentYear = 2022)
    {
        var student = CreateStudent(enrollmentYear);
        student.Expel(expulsionDate, reason);
        return student;
    }

    public static Group CreateGroup(Guid? specialtyId = null) =>
        Group.Create("Б2F301ДОФ24", specialtyId ?? Guid.CreateVersion7());

    public static Student CreateStudentInGroup(Group group)
    {
        var student = CreateStudent(groupId: group.Id);
        typeof(Student).GetProperty(nameof(Student.Group))!.SetValue(student, group);
        return student;
    }

    public static Direction CreateDirectionWithSpecialties(string directionCode, params string[] specialtyCodes)
    {
        var direction = Direction.Create(directionCode, "Інформаційні технології", "Бакалавр, магістр");
        var specialties = (List<Specialty>)typeof(Direction)
            .GetField("_specialties", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(direction)!;

        foreach (var code in specialtyCodes)
            specialties.Add(Specialty.Create(code, "Фах", "Бакалавр", direction.Id, Guid.CreateVersion7()));

        return direction;
    }
}
