using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Enums;
using DekanPlus.Domain.Interfaces.Repositories;
using DekanPlus.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DekanPlus.Infrastructure.Persistence.Repositories;

public class StudentRepository(DekanPlusDbContext dbContext) : IStudentRepository
{
    public Task<Student?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Students
            .Include(s => s.Group).ThenInclude(g => g.Specialty).ThenInclude(sp => sp.Direction)
            .Include(s => s.Group).ThenInclude(g => g.Specialty).ThenInclude(sp => sp.Department)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<Student>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Students
            .AsNoTracking()
            .Include(s => s.Group).ThenInclude(g => g.Specialty)
            .ToListAsync(cancellationToken);

    public Task<bool> ExistsActiveAsync(FullName fullName, DateOnly birthDate, CancellationToken cancellationToken = default) =>
        dbContext.Students.AnyAsync(s =>
            s.Expulsion == null &&
            s.BirthDate == birthDate &&
            s.FullName.LastName == fullName.LastName &&
            s.FullName.FirstName == fullName.FirstName &&
            s.FullName.MiddleName == fullName.MiddleName,
            cancellationToken);

    public async Task AddAsync(Student student, CancellationToken cancellationToken = default) =>
        await dbContext.Students.AddAsync(student, cancellationToken);

    // Задача 2
    public async Task<IReadOnlyCollection<Student>> GetExpelledAsync(
        ExpulsionReason? reason = null,
        DateOnly? from = null,
        DateOnly? to = null,
        Guid? groupId = null,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Students
            .AsNoTracking()
            .Include(s => s.Group).ThenInclude(g => g.Specialty)
            .Where(s => s.Expulsion != null);

        if (reason is { } expulsionReason)
            query = query.Where(s => s.Expulsion!.Reason == expulsionReason);

        if (from is { } fromDate)
            query = query.Where(s => s.Expulsion!.Date >= fromDate);

        if (to is { } toDate)
            query = query.Where(s => s.Expulsion!.Date <= toDate);

        if (groupId is { } id)
            query = query.Where(s => s.GroupId == id);

        return await query
            .OrderByDescending(s => s.Expulsion!.Date)
            .ToListAsync(cancellationToken);
    }
}
