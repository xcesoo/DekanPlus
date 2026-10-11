using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Application.Common.Rules.Implementations.Students;

public class StudentAgeRule(TimeProvider timeProvider, StudentRulesOptions options) : IBusinessRule
{
    private DateOnly Today => DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime);

    public RuleResult Apply(IRuleContext context) => context switch
    {
        EnrollStudentContext ctx when ctx.BirthDate > Today
            => RuleResult.Failed("Дата народження не може бути в майбутньому."),

        EnrollStudentContext { EnrollmentYear: >= 1 and <= 9999 } ctx when AgeAtAdmission(ctx) < options.MinEnrollmentAge
            => RuleResult.Failed($"Вік на момент вступу має бути не меншим за {options.MinEnrollmentAge} років."),

        EnrollStudentContext { EnrollmentYear: >= 1 and <= 9999 } ctx when AgeAtAdmission(ctx) > options.MaxEnrollmentAge
            => RuleResult.Failed($"Вік на момент вступу не може перевищувати {options.MaxEnrollmentAge} років."),

        _ => RuleResult.Ok()
    };

    private static int AgeAtAdmission(EnrollStudentContext ctx)
    {
        var admissionDate = new DateOnly(ctx.EnrollmentYear, 9, 1);
        var age = admissionDate.Year - ctx.BirthDate.Year;

        return ctx.BirthDate.AddYears(age) > admissionDate ? age - 1 : age;
    }
}
