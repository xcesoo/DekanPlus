namespace DekanPlus.Application.Common.Rules;

public class StudentRulesOptions
{
    public const string SectionName = "StudentRules";

    public int MinEnrollmentAge { get; set; } = 16;
    public int MinEnrollmentYear { get; set; } = 1900;
    public int MinStudyYearsForGraduation { get; set; } = 3;
    public int ReinstatementWindowYears { get; set; } = 3;
    public bool AllowTransferBetweenSpecialties { get; set; } = false;
}
