using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Application.Common.Rules.Implementations.Students;

public class ExpulsionDateRule(TimeProvider timeProvider) : IBusinessRule
{
    private DateOnly Today => DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime);

    public RuleResult Apply(IRuleContext context) => context switch
    {
        ExpelStudentContext ctx when ctx.Date > Today
            => RuleResult.Failed("Дата відрахування не може бути в майбутньому."),

        ExpelStudentContext ctx when ctx.Date < new DateOnly(ctx.Student.EnrollmentYear, 9, 1)
            => RuleResult.Failed("Дата відрахування не може передувати вступу."),

        _ => RuleResult.Ok()
    };
}
