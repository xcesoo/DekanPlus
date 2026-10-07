using DekanPlus.Domain.Entities;

namespace DekanPlus.Domain.Interfaces.Repositories;

public interface ISpecialtyRepository
{
    Task<Specialty?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Specialty?> GetByStateCodeAsync(string stateCode, CancellationToken cancellationToken = default);
    Task AddAsync(Specialty specialty, CancellationToken cancellationToken = default);
    Task DeleteAsync(Specialty specialty, CancellationToken cancellationToken = default);

    // Задача 4
    Task<IReadOnlyCollection<Specialty>> GetAllAsync(CancellationToken cancellationToken = default);
}
