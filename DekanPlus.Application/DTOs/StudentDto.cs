using DekanPlus.Domain.Enums;

namespace DekanPlus.Application.DTOs;

public record StudentDto(
    Guid Id,
    string LastName,
    string FirstName,
    string? MiddleName,
    DateOnly BirthDate,
    string HomeAddress,
    string? ResidenceAddress,
    int EnrollmentYear,
    StudyForm StudyForm,
    string RecordBookNumber,
    Guid GroupId,
    string GroupName,
    DateOnly? ExpulsionDate,
    ExpulsionReason? ExpulsionReason);
