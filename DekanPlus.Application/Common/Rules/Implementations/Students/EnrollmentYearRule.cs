using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Application.Common.Rules.Implementations.Students;

public class EnrollmentYearRule(TimeProvider timeProvider, StudentRulesOptions options) : IBusinessRule
{
    public RuleResult Apply(IRuleContext context) => context switch
    {
        EnrollStudentContext ctx when ctx.EnrollmentYear > timeProvider.GetLocalNow().Year
            => RuleResult.Failed("Рік вступу не може бути з майбутнього."),

        EnrollStudentContext ctx when ctx.EnrollmentYear < options.MinEnrollmentYear
            => RuleResult.Failed($"Рік вступу не може бути меншим за {options.MinEnrollmentYear}."),

        _ => RuleResult.Ok()
    };
}
