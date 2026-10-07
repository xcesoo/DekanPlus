using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Application.Common.Rules.Implementations.Students;

public class ExpulsionDateRule(TimeProvider timeProvider) : IBusinessRule
{
    public RuleResult Apply(IRuleContext context)
    {
        if (context is not ExpelStudentContext ctx)
            return RuleResult.Ok();

        var today = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);
        if (ctx.Date > today)
            return RuleResult.Failed("Дата відрахування не може бути в майбутньому.");

        var admissionDate = new DateOnly(ctx.Student.EnrollmentYear, 9, 1);
        if (ctx.Date < admissionDate)
            return RuleResult.Failed("Дата відрахування не може передувати вступу.");

        return RuleResult.Ok();
    }
}
