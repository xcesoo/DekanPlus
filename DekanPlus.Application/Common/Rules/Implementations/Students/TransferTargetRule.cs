using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Application.Common.Rules.Implementations.Students;

public class TransferTargetRule : IBusinessRule
{
    public RuleResult Apply(IRuleContext context) => context switch
    {
        TransferStudentContext ctx when ctx.TargetGroup.Id == ctx.Student.GroupId
            => RuleResult.Failed("Студент уже навчається в цій групі."),

        _ => RuleResult.Ok()
    };
}
