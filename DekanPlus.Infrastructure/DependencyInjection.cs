using DekanPlus.Domain.Interfaces.Repositories;
using DekanPlus.Infrastructure.Persistence;
using DekanPlus.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DekanPlus.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DekanPlusDbContext>(o =>
        {
            o.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<DekanPlusDbContext>());

        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IDirectionRepository, DirectionRepository>();
        services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();

        return services;
    }
}
