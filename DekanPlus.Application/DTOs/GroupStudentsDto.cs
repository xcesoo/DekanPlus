namespace DekanPlus.Application.DTOs;

public record GroupStudentsDto(
    Guid GroupId,
    string GroupName,
    string SpecialtyStateCode,
    string SpecialtyName,
    string DirectionName,
    string DepartmentName,
    int StudentsCount,
    IReadOnlyCollection<GroupStudentDto> Students);
