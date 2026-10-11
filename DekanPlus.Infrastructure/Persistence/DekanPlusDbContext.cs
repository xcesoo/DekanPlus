using DekanPlus.Application.Exceptions;
using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Interfaces.Repositories;
using DekanPlus.Infrastructure.Persistence.Configurations;
using DekanPlus.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DekanPlus.Infrastructure.Persistence;

public class DekanPlusDbContext : DbContext, IUnitOfWork
{
    public DekanPlusDbContext(DbContextOptions<DekanPlusDbContext> options) : base(options) { }

    public DbSet<Department> Departments { get; set; }
    public DbSet<Direction> Directions { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DepartmentConfiguration).Assembly);
        modelBuilder.HasPostgresExtension("pg_trgm");
        modelBuilder.HasSequence<long>(RecordBookNumberGenerator.SequenceName);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyConflictException("Запис було змінено іншим користувачем. Оновіть дані та повторіть операцію.");
        }
        catch (Exception ex) when (GetPostgresException(ex) is { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new DuplicateValueException("Значення, що вводиться, вже використовується.");
        }
        catch (Exception ex) when (GetPostgresException(ex) is { SqlState: PostgresErrorCodes.ForeignKeyViolation })
        {
            throw new EntityInUseException("Операція порушує зв'язки даних: запис використовується іншими даними (групами, студентами тощо) або посилається на неіснуючий запис.");
        }
    }

    private static PostgresException? GetPostgresException(Exception ex) =>
        ex as PostgresException ?? ex.InnerException as PostgresException;
}
