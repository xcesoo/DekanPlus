using DekanPlus.Domain.Enums;

namespace DekanPlus.Application.DTOs;

public record ExpelledStudentsByReasonDto(
    ExpulsionReason Reason,
    int Count,
    IReadOnlyCollection<StudentDto> Students);
