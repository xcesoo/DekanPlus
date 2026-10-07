using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DekanPlus.Infrastructure.Persistence.Repositories;

public class DepartmentRepository(DekanPlusDbContext dbContext) : IDepartmentRepository
{
    public Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Departments.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<Department>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Departments.AsNoTracking().ToListAsync(cancellationToken);

    public Task<Department?> GetByNameAsync(string name, CancellationToken cancellationToken = default) =>
        dbContext.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.Name == name, cancellationToken);

    public async Task AddAsync(Department department, CancellationToken cancellationToken = default) =>
        await dbContext.Departments.AddAsync(department, cancellationToken);

    public Task DeleteAsync(Department department, CancellationToken cancellationToken = default)
    {
        dbContext.Departments.Remove(department);
        return Task.CompletedTask;
    }
}
