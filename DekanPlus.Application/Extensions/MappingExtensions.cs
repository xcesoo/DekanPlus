using DekanPlus.Application.DTOs;
using DekanPlus.Domain.Entities;

namespace DekanPlus.Application.Extensions;

internal static class MappingExtensions
{
    public static DepartmentDto MapToDto(this Department department) =>
        new(department.Id, department.Name);

    public static DirectionDto MapToDto(this Direction direction) =>
        new(direction.Id, direction.StateCode, direction.Name, direction.Qualification);

    public static SpecialtyDto MapToDto(this Specialty specialty) =>
        new(
            specialty.Id,
            specialty.StateCode,
            specialty.Name,
            specialty.Qualification,
            specialty.DirectionId,
            specialty.Direction.Name,
            specialty.DepartmentId,
            specialty.Department.Name);

    public static GroupDto MapToDto(this Group group) =>
        new(group.Id, group.Name, group.SpecialtyId, group.Specialty.Name);

    public static StudentDto MapToDto(this Student student) =>
        new(
            student.Id,
            student.FullName.LastName,
            student.FullName.FirstName,
            student.FullName.MiddleName,
            student.BirthDate,
            student.HomeAddress,
            student.ResidenceAddress,
            student.EnrollmentYear,
            student.StudyForm,
            student.RecordBookNumber,
            student.GroupId,
            student.Group.Name,
            student.Expulsion?.Date,
            student.Expulsion?.Reason);

    public static GroupStudentDto MapToGroupStudentDto(this Student student) =>
        new(
            student.Id,
            student.FullName.LastName,
            student.FullName.FirstName,
            student.FullName.MiddleName,
            student.BirthDate,
            student.StudyForm,
            student.RecordBookNumber,
            student.Expulsion?.Date,
            student.Expulsion?.Reason);
}
