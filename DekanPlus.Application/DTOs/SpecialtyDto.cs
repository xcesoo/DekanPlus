namespace DekanPlus.Application.DTOs;

public record SpecialtyDto(
    Guid Id,
    string StateCode,
    string Name,
    string Qualification,
    Guid DirectionId,
    string DirectionName,
    Guid DepartmentId,
    string DepartmentName);
