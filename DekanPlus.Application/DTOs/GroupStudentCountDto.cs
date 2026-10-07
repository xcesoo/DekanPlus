namespace DekanPlus.Application.DTOs;

public record GroupStudentCountDto(
    Guid GroupId,
    string GroupName,
    string SpecialtyName,
    int ActiveStudents,
    int ExpelledStudents);
