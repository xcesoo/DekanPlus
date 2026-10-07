using DekanPlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DekanPlus.Infrastructure.Persistence.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("students");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.OwnsOne(s => s.FullName, fn =>
        {
            fn.Property(f => f.LastName)
                .HasColumnName("last_name")
                .IsRequired()
                .HasMaxLength(100);

            fn.Property(f => f.FirstName)
                .HasColumnName("first_name")
                .IsRequired()
                .HasMaxLength(100);

            fn.Property(f => f.MiddleName)
                .HasColumnName("middle_name")
                .HasMaxLength(100);

            fn.HasIndex(f => f.LastName, "ix_students_last_name_trgm")
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");

            fn.HasIndex(f => f.FirstName, "ix_students_first_name_trgm")
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");
        });

        builder.Navigation(s => s.FullName).IsRequired();

        builder.Property(s => s.BirthDate)
            .HasColumnName("birth_date");

        builder.Property(s => s.HomeAddress)
            .HasColumnName("home_address")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(s => s.ResidenceAddress)
            .HasColumnName("residence_address")
            .HasMaxLength(500);

        builder.Property(s => s.EnrollmentYear)
            .HasColumnName("enrollment_year");

        builder.Property(s => s.StudyForm)
            .HasColumnName("study_form")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(s => s.RecordBookNumber)
            .HasColumnName("record_book_number")
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(s => s.RecordBookNumber, "ix_students_record_book_number_unique")
            .IsUnique();

        builder.OwnsOne(s => s.Expulsion, ex =>
        {
            ex.Property(e => e.Date)
                .HasColumnName("expulsion_date");

            ex.Property(e => e.Reason)
                .HasColumnName("expulsion_reason")
                .HasConversion<string>();
        });

        builder.Property(s => s.GroupId)
            .HasColumnName("group_id");

        builder.HasOne(s => s.Group)
            .WithMany(g => g.Students)
            .HasForeignKey(s => s.GroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
