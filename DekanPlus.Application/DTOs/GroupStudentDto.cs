using DekanPlus.Domain.Enums;

namespace DekanPlus.Application.DTOs;

public record GroupStudentDto(
    Guid Id,
    string LastName,
    string FirstName,
    string? MiddleName,
    DateOnly BirthDate,
    StudyForm StudyForm,
    string RecordBookNumber,
    DateOnly? ExpulsionDate,
    ExpulsionReason? ExpulsionReason);
