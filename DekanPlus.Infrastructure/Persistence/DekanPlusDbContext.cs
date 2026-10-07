using DekanPlus.Application.Exceptions;
using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Interfaces.Repositories;
using DekanPlus.Infrastructure.Persistence.Configurations;
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
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (GetPostgresException(ex) is { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new DuplicateValueException("Значення, що вводиться, вже використовується.");
        }
        catch (Exception ex) when (GetPostgresException(ex) is { SqlState: PostgresErrorCodes.ForeignKeyViolation })
        {
            throw new EntityInUseException("Неможливо видалити запис: він використовується в інших даних (групах, студентах тощо).");
        }
    }

    private static PostgresException? GetPostgresException(Exception ex) =>
        ex as PostgresException ?? ex.InnerException as PostgresException;
}
