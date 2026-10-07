using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DekanPlus.Infrastructure.Persistence.Repositories;

public class GroupRepository(DekanPlusDbContext dbContext) : IGroupRepository
{
    public Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Groups
            .Include(g => g.Specialty)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<Group>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Groups
            .AsNoTracking()
            .Include(g => g.Specialty)
            .ToListAsync(cancellationToken);

    public Task<Group?> GetByNameAsync(string name, CancellationToken cancellationToken = default) =>
        dbContext.Groups.AsNoTracking().FirstOrDefaultAsync(g => g.Name == name, cancellationToken);

    public async Task AddAsync(Group group, CancellationToken cancellationToken = default) =>
        await dbContext.Groups.AddAsync(group, cancellationToken);

    public Task DeleteAsync(Group group, CancellationToken cancellationToken = default)
    {
        dbContext.Groups.Remove(group);
        return Task.CompletedTask;
    }

    // Задача 1
    public Task<Group?> GetWithStudentsAsync(Guid id, bool includeExpelled, CancellationToken cancellationToken = default) =>
        dbContext.Groups
            .AsNoTracking()
            .Include(g => g.Specialty).ThenInclude(s => s.Direction)
            .Include(g => g.Specialty).ThenInclude(s => s.Department)
            .Include(g => g.Students.Where(s => includeExpelled || s.Expulsion == null))
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

    // Задача 3
    public async Task<IReadOnlyCollection<Group>> GetAllWithStudentsAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Groups
            .AsNoTracking()
            .Include(g => g.Specialty)
            .Include(g => g.Students)
            .ToListAsync(cancellationToken);
}
