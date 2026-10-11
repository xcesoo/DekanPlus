namespace DekanPlus.WebApi.Models;

/// <summary>Відповідь на успішне створення запису.</summary>
/// <param name="Id">Ідентифікатор створеного запису.</param>
public record CreatedResponse(Guid Id);
