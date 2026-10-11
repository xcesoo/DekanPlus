namespace DekanPlus.Application.DTOs;

/// <summary>Група.</summary>
/// <param name="Id">Ідентифікатор групи.</param>
/// <param name="Name">Назва групи.</param>
/// <param name="SpecialtyId">Ідентифікатор фаху.</param>
/// <param name="SpecialtyName">Назва фаху групи.</param>
public record GroupDto(Guid Id, string Name, Guid SpecialtyId, string SpecialtyName);
