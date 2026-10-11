namespace DekanPlus.Application.DTOs;

/// <summary>Список студентів групи з даними про фах, напрям та кафедру.</summary>
/// <param name="GroupId">Ідентифікатор групи.</param>
/// <param name="GroupName">Назва групи.</param>
/// <param name="SpecialtyStateCode">Державний номер фаху.</param>
/// <param name="SpecialtyName">Назва фаху.</param>
/// <param name="DirectionName">Назва напряму підготовки.</param>
/// <param name="DepartmentName">Назва кафедри.</param>
/// <param name="StudentsCount">Кількість студентів у списку.</param>
/// <param name="Students">Студенти групи (за абеткою).</param>
public record GroupStudentsDto(
    Guid GroupId,
    string GroupName,
    string SpecialtyStateCode,
    string SpecialtyName,
    string DirectionName,
    string DepartmentName,
    int StudentsCount,
    IReadOnlyCollection<GroupStudentDto> Students);
