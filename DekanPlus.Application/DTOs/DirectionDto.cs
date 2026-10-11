namespace DekanPlus.Application.DTOs;

/// <summary>Напрям підготовки.</summary>
/// <param name="Id">Ідентифікатор напряму підготовки.</param>
/// <param name="StateCode">Державний номер напряму підготовки.</param>
/// <param name="Name">Назва напряму підготовки.</param>
/// <param name="Qualification">Кваліфікація, що присуджується випускнику.</param>
public record DirectionDto(Guid Id, string StateCode, string Name, string Qualification);
