using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DekanPlus.Infrastructure.Persistence.Repositories;

public class SpecialtyRepository(DekanPlusDbContext dbContext) : ISpecialtyRepository
{
    public Task<Specialty?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Specialties
            .Include(s => s.Direction)
            .Include(s => s.Department)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public Task<Specialty?> GetByStateCodeAsync(string stateCode, CancellationToken cancellationToken = default) =>
        dbContext.Specialties.AsNoTracking().FirstOrDefaultAsync(s => s.StateCode == stateCode, cancellationToken);

    public async Task AddAsync(Specialty specialty, CancellationToken cancellationToken = default) =>
        await dbContext.Specialties.AddAsync(specialty, cancellationToken);

    public Task DeleteAsync(Specialty specialty, CancellationToken cancellationToken = default)
    {
        dbContext.Specialties.Remove(specialty);
        return Task.CompletedTask;
    }

    // Задача 4
    public async Task<IReadOnlyCollection<Specialty>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Specialties
            .AsNoTracking()
            .Include(s => s.Direction)
            .Include(s => s.Department)
            .OrderBy(s => s.StateCode)
            .ToListAsync(cancellationToken);
}
