using DekanPlus.Domain.Entities;

namespace DekanPlus.Domain.Interfaces.Repositories;

public interface IDirectionRepository
{
    Task<Direction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Direction>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Direction?> GetByStateCodeAsync(string stateCode, CancellationToken cancellationToken = default);
    Task AddAsync(Direction direction, CancellationToken cancellationToken = default);
    Task DeleteAsync(Direction direction, CancellationToken cancellationToken = default);
}
