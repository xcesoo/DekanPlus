using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Application.Common.Rules.Implementations.Students;

public class TransferSameSpecialtyRule(StudentRulesOptions options) : IBusinessRule
{
    public RuleResult Apply(IRuleContext context) => context switch
    {
        TransferStudentContext ctx
            when !options.AllowTransferBetweenSpecialties
                 && ctx.Student.Group.SpecialtyId != ctx.TargetGroup.SpecialtyId
            => RuleResult.Failed("Переведення між різними фахами не дозволено."),

        _ => RuleResult.Ok()
    };
}
