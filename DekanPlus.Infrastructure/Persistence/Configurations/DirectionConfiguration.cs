using DekanPlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DekanPlus.Infrastructure.Persistence.Configurations;

public class DirectionConfiguration : IEntityTypeConfiguration<Direction>
{
    public void Configure(EntityTypeBuilder<Direction> builder)
    {
        builder.ToTable("directions");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(d => d.StateCode)
            .HasColumnName("state_code")
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(d => d.StateCode, "ix_directions_state_code_unique")
            .IsUnique();

        builder.Property(d => d.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(d => d.Qualification)
            .HasColumnName("qualification")
            .IsRequired()
            .HasMaxLength(300);

        builder.Metadata
            .FindNavigation(nameof(Direction.Specialties))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
