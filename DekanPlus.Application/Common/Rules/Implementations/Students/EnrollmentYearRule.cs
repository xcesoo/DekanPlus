using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Application.Common.Rules.Implementations.Students;

public class EnrollmentYearRule(TimeProvider timeProvider, StudentRulesOptions options) : IBusinessRule
{
    public RuleResult Apply(IRuleContext context)
    {
        if (context is not EnrollStudentContext ctx)
            return RuleResult.Ok();

        var currentYear = timeProvider.GetUtcNow().UtcDateTime.Year;

        if (ctx.EnrollmentYear > currentYear)
            return RuleResult.Failed("Рік вступу не може бути з майбутнього.");

        if (ctx.EnrollmentYear < options.MinEnrollmentYear)
            return RuleResult.Failed($"Рік вступу не може бути меншим за {options.MinEnrollmentYear}.");

        return RuleResult.Ok();
    }
}
