using DekanPlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DekanPlus.Infrastructure.Persistence.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("groups");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(g => g.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(g => g.Name, "ix_groups_name_unique")
            .IsUnique();

        builder.Property(g => g.SpecialtyId)
            .HasColumnName("specialty_id");

        builder.HasOne(g => g.Specialty)
            .WithMany(s => s.Groups)
            .HasForeignKey(g => g.SpecialtyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Metadata
            .FindNavigation(nameof(Group.Students))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
