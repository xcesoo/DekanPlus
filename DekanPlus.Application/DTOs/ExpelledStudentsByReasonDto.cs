using DekanPlus.Domain.Enums;

namespace DekanPlus.Application.DTOs;

/// <summary>Відраховані студенти, згруповані за причиною відрахування.</summary>
/// <param name="Reason">Причина відрахування.</param>
/// <param name="Count">Кількість студентів.</param>
/// <param name="Students">Відраховані студенти.</param>
public record ExpelledStudentsByReasonDto(
    ExpulsionReason Reason,
    int Count,
    IReadOnlyCollection<StudentDto> Students);
