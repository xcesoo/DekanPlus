using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Application.Common.Rules.Implementations.Specialties;

public class SpecialtyCodeMatchesDirectionRule : IBusinessRule
{
    public RuleResult Apply(IRuleContext context)
    {
        var (direction, stateCode) = context switch
        {
            CreateSpecialtyContext ctx => (ctx.Direction, ctx.StateCode),
            ChangeSpecialtyStateCodeContext ctx => (ctx.Direction, ctx.StateCode),
            _ => (null, null)
        };

        if (direction is null || stateCode is null)
            return RuleResult.Ok();

        if (!stateCode.Trim().StartsWith(direction.StateCode, StringComparison.Ordinal))
            return RuleResult.Failed("Код фаху має починатися з коду напряму підготовки.");

        return RuleResult.Ok();
    }
}
