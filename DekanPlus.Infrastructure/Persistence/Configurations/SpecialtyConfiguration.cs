using DekanPlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DekanPlus.Infrastructure.Persistence.Configurations;

public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
{
    public void Configure(EntityTypeBuilder<Specialty> builder)
    {
        builder.ToTable("specialties");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(s => s.StateCode)
            .HasColumnName("state_code")
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(s => s.StateCode, "ix_specialties_state_code_unique")
            .IsUnique();

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(s => s.Qualification)
            .HasColumnName("qualification")
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(s => s.DirectionId)
            .HasColumnName("direction_id");

        builder.Property(s => s.DepartmentId)
            .HasColumnName("department_id");

        builder.HasOne(s => s.Direction)
            .WithMany(d => d.Specialties)
            .HasForeignKey(s => s.DirectionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Department)
            .WithMany(d => d.Specialties)
            .HasForeignKey(s => s.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Metadata
            .FindNavigation(nameof(Specialty.Groups))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
