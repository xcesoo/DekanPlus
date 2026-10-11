namespace DekanPlus.Application.DTOs;

/// <summary>Кількість студентів у групі.</summary>
/// <param name="GroupId">Ідентифікатор групи.</param>
/// <param name="GroupName">Назва групи.</param>
/// <param name="SpecialtyName">Назва фаху групи.</param>
/// <param name="ActiveStudents">Кількість студентів, які навчаються.</param>
/// <param name="ExpelledStudents">Кількість відрахованих студентів.</param>
public record GroupStudentCountDto(
    Guid GroupId,
    string GroupName,
    string SpecialtyName,
    int ActiveStudents,
    int ExpelledStudents);
