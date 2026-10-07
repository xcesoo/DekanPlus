using DekanPlus.Domain.Entities;

namespace DekanPlus.Domain.Interfaces.Repositories;

public interface IGroupRepository
{
    Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Group>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Group?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task AddAsync(Group group, CancellationToken cancellationToken = default);
    Task DeleteAsync(Group group, CancellationToken cancellationToken = default);

    // Задача 1
    Task<Group?> GetWithStudentsAsync(Guid id, bool includeExpelled, CancellationToken cancellationToken = default);

    // Задача 3
    Task<IReadOnlyCollection<Group>> GetAllWithStudentsAsync(CancellationToken cancellationToken = default);
}
