namespace DekanPlus.Application.DTOs;

/// <summary>Кафедра.</summary>
/// <param name="Id">Ідентифікатор кафедри.</param>
/// <param name="Name">Назва кафедри.</param>
public record DepartmentDto(Guid Id, string Name);
