using DekanPlus.Domain.Enums;

namespace DekanPlus.Application.DTOs;

/// <summary>Студент у списку групи.</summary>
/// <param name="Id">Ідентифікатор студента.</param>
/// <param name="LastName">Прізвище.</param>
/// <param name="FirstName">Ім'я.</param>
/// <param name="MiddleName">По батькові (необов'язково).</param>
/// <param name="BirthDate">Дата народження.</param>
/// <param name="StudyForm">Вид навчання.</param>
/// <param name="RecordBookNumber">Номер залікової книжки.</param>
/// <param name="ExpulsionDate">Дата відрахування (null, якщо студент навчається).</param>
/// <param name="ExpulsionReason">Причина відрахування (null, якщо студент навчається).</param>
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
