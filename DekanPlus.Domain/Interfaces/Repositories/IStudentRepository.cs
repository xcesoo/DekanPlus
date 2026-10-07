using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Enums;
using DekanPlus.Domain.ValueObjects;

namespace DekanPlus.Domain.Interfaces.Repositories;

public interface IStudentRepository
{
    Task<Student?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Student>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsActiveAsync(FullName fullName, DateOnly birthDate, CancellationToken cancellationToken = default);
    Task AddAsync(Student student, CancellationToken cancellationToken = default);

    // Задача 2
    Task<IReadOnlyCollection<Student>> GetExpelledAsync(
        ExpulsionReason? reason = null,
        DateOnly? from = null,
        DateOnly? to = null,
        Guid? groupId = null,
        CancellationToken cancellationToken = default);
}
