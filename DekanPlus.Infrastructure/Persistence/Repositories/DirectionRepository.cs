using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DekanPlus.Infrastructure.Persistence.Repositories;

public class DirectionRepository(DekanPlusDbContext dbContext) : IDirectionRepository
{
    public Task<Direction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Directions.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<Direction>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Directions.AsNoTracking().OrderBy(d => d.StateCode).ToListAsync(cancellationToken);

    public Task<Direction?> GetByStateCodeAsync(string stateCode, CancellationToken cancellationToken = default) =>
        dbContext.Directions.AsNoTracking().FirstOrDefaultAsync(d => d.StateCode == stateCode, cancellationToken);

    public async Task AddAsync(Direction direction, CancellationToken cancellationToken = default) =>
        await dbContext.Directions.AddAsync(direction, cancellationToken);

    public Task DeleteAsync(Direction direction, CancellationToken cancellationToken = default)
    {
        dbContext.Directions.Remove(direction);
        return Task.CompletedTask;
    }
}
