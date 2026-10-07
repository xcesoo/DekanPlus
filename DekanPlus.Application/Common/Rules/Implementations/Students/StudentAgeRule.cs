using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Application.Common.Rules.Implementations.Students;

public class StudentAgeRule(TimeProvider timeProvider, StudentRulesOptions options) : IBusinessRule
{
    public RuleResult Apply(IRuleContext context)
    {
        if (context is not EnrollStudentContext ctx)
            return RuleResult.Ok();

        var today = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);
        if (ctx.BirthDate > today)
            return RuleResult.Failed("Дата народження не може бути в майбутньому.");

        if (ctx.EnrollmentYear is < 1 or > 9999)
            return RuleResult.Ok();

        var admissionDate = new DateOnly(ctx.EnrollmentYear, 9, 1);
        var age = admissionDate.Year - ctx.BirthDate.Year;
        if (ctx.BirthDate.AddYears(age) > admissionDate)
            age--;

        if (age < options.MinEnrollmentAge)
            return RuleResult.Failed($"Вік на момент вступу має бути не меншим за {options.MinEnrollmentAge} років.");

        return RuleResult.Ok();
    }
}
