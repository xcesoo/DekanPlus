using DekanPlus.Domain.Enums;

namespace DekanPlus.Application.DTOs;

/// <summary>Особова картка студента.</summary>
/// <param name="Id">Ідентифікатор студента.</param>
/// <param name="LastName">Прізвище.</param>
/// <param name="FirstName">Ім'я.</param>
/// <param name="MiddleName">По батькові (необов'язково).</param>
/// <param name="BirthDate">Дата народження.</param>
/// <param name="HomeAddress">Домашня адреса.</param>
/// <param name="ResidenceAddress">Адреса проживання (необов'язково; якщо не вказана, збігається з домашньою).</param>
/// <param name="EnrollmentYear">Рік вступу до ВНЗ.</param>
/// <param name="StudyForm">Вид навчання.</param>
/// <param name="RecordBookNumber">Номер залікової книжки.</param>
/// <param name="GroupId">Ідентифікатор групи студента.</param>
/// <param name="GroupName">Назва групи.</param>
/// <param name="ExpulsionDate">Дата відрахування (null, якщо студент навчається).</param>
/// <param name="ExpulsionReason">Причина відрахування (null, якщо студент навчається).</param>
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
