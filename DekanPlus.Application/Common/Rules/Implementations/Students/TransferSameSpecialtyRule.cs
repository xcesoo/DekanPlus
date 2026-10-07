using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Application.Common.Rules.Implementations.Students;

public class TransferSameSpecialtyRule(StudentRulesOptions options) : IBusinessRule
{
    public RuleResult Apply(IRuleContext context)
    {
        if (context is not TransferStudentContext ctx || options.AllowTransferBetweenSpecialties)
            return RuleResult.Ok();

        if (ctx.Student.Group.SpecialtyId != ctx.TargetGroup.SpecialtyId)
            return RuleResult.Failed("Переведення між різними фахами не дозволено.");

        return RuleResult.Ok();
    }
}
