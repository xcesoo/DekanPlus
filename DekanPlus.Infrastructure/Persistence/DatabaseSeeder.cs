using System.Text.Json;
using System.Text.Json.Serialization;
using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Enums;
using DekanPlus.Domain.ValueObjects;
using DekanPlus.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DekanPlus.Infrastructure.Persistence;

public class DatabaseSeeder
{
    private class DepartmentSeedDto
    {
        public string Name { get; set; } = string.Empty;
    }

    private class DirectionSeedDto
    {
        public string StateCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
    }

    private class SpecialtySeedDto
    {
        public string StateCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public string DirectionCode { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
    }

    private class GroupSeedDto
    {
        public string Name { get; set; } = string.Empty;
        public string SpecialtyCode { get; set; } = string.Empty;
    }

    private class StudentSeedDto
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public DateOnly BirthDate { get; set; }
        public string HomeAddress { get; set; } = string.Empty;
        public string? ResidenceAddress { get; set; }
        public int EnrollmentYear { get; set; }
        public StudyForm StudyForm { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public DateOnly? ExpulsionDate { get; set; }
        public ExpulsionReason? ExpulsionReason { get; set; }
    }

    private class SeedDataRoot
    {
        public List<DepartmentSeedDto>? Departments { get; set; }
        public List<DirectionSeedDto>? Directions { get; set; }
        public List<SpecialtySeedDto>? Specialties { get; set; }
        public List<GroupSeedDto>? Groups { get; set; }
        public List<StudentSeedDto>? Students { get; set; }
    }

    public static async Task SeedAsync(DekanPlusDbContext context, ILogger logger)
    {
        await context.Database.MigrateAsync();

        if (await context.Departments.AnyAsync())
        {
            logger.LogInformation("DB has already been seeded.");
            return;
        }

        var filePath = Path.Combine(AppContext.BaseDirectory, "DbSeed.json");
        if (!File.Exists(filePath))
        {
            logger.LogError("File not found. Path: {Path}", filePath);
            return;
        }

        var seedData = await File.ReadAllTextAsync(filePath);
        var parsedData = JsonSerializer.Deserialize<SeedDataRoot>(seedData, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        });

        if (parsedData is null)
        {
            logger.LogError("Failed to deserialize seed data.");
            return;
        }

        var departments = (parsedData.Departments ?? [])
            .Select(dto => Department.Create(dto.Name))
            .ToDictionary(d => d.Name);

        var directions = (parsedData.Directions ?? [])
            .Select(dto => Direction.Create(dto.StateCode, dto.Name, dto.Qualification))
            .ToDictionary(d => d.StateCode);

        var specialties = (parsedData.Specialties ?? [])
            .Select(dto => Specialty.Create(
                dto.StateCode, dto.Name, dto.Qualification,
                directions[dto.DirectionCode].Id,
                departments[dto.DepartmentName].Id))
            .ToDictionary(s => s.StateCode);

        var groups = (parsedData.Groups ?? [])
            .Select(dto => Group.Create(dto.Name, specialties[dto.SpecialtyCode].Id))
            .ToDictionary(g => g.Name);

        var groupSpecialtyCodes = (parsedData.Groups ?? []).ToDictionary(g => g.Name, g => g.SpecialtyCode);
        var recordBookNumberGenerator = new RecordBookNumberGenerator(context);
        var students = new List<Student>();

        foreach (var dto in parsedData.Students ?? [])
        {
            var group = groups[dto.GroupName];
            var recordBookNumber = await recordBookNumberGenerator.GenerateAsync(
                groupSpecialtyCodes[dto.GroupName], dto.EnrollmentYear);

            var student = Student.Create(
                FullName.Create(dto.LastName, dto.FirstName, dto.MiddleName),
                dto.BirthDate,
                dto.HomeAddress,
                dto.ResidenceAddress,
                dto.EnrollmentYear,
                dto.StudyForm,
                recordBookNumber,
                group.Id);

            if (dto.ExpulsionDate is { } expulsionDate && dto.ExpulsionReason is { } expulsionReason)
                student.Expel(expulsionDate, expulsionReason);

            students.Add(student);
        }

        await context.Departments.AddRangeAsync(departments.Values);
        await context.Directions.AddRangeAsync(directions.Values);
        await context.Specialties.AddRangeAsync(specialties.Values);
        await context.Groups.AddRangeAsync(groups.Values);
        await context.Students.AddRangeAsync(students);

        await context.SaveChangesAsync();

        logger.LogInformation(
            "Seeded {Departments} departments, {Directions} directions, {Specialties} specialties, {Groups} groups, {Students} students.",
            departments.Count, directions.Count, specialties.Count, groups.Count, students.Count);
    }
}
