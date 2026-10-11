namespace DekanPlus.Application.DTOs;

/// <summary>Фах (спеціальність) з напрямом підготовки та випускаючою кафедрою.</summary>
/// <param name="Id">Ідентифікатор фаху.</param>
/// <param name="StateCode">Державний номер фаху.</param>
/// <param name="Name">Назва фаху.</param>
/// <param name="Qualification">Кваліфікація, що присуджується випускнику.</param>
/// <param name="DirectionId">Ідентифікатор напряму підготовки.</param>
/// <param name="DirectionName">Назва напряму підготовки.</param>
/// <param name="DepartmentId">Ідентифікатор випускаючої кафедри.</param>
/// <param name="DepartmentName">Назва кафедри.</param>
public record SpecialtyDto(
    Guid Id,
    string StateCode,
    string Name,
    string Qualification,
    Guid DirectionId,
    string DirectionName,
    Guid DepartmentId,
    string DepartmentName);
