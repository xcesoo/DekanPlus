using DekanPlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DekanPlus.Infrastructure.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(d => d.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(300);

        builder.HasIndex(d => d.Name, "ix_departments_name_unique")
            .IsUnique();

        builder.Metadata
            .FindNavigation(nameof(Department.Specialties))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
