using DekanPlus.Application.Common.Rules.Interfaces;
using DekanPlus.Domain.Entities;

namespace DekanPlus.Application.Common.Rules.Implementations.Specialties;

public class SpecialtyCodeMatchesDirectionRule : IBusinessRule
{
    public RuleResult Apply(IRuleContext context) => context switch
    {
        CreateSpecialtyContext { StateCode: not null } ctx when !StartsWithDirectionCode(ctx.Direction, ctx.StateCode)
            => RuleResult.Failed("Код фаху має починатися з коду напряму підготовки."),

        ChangeSpecialtyStateCodeContext { StateCode: not null } ctx when !StartsWithDirectionCode(ctx.Direction, ctx.StateCode)
            => RuleResult.Failed("Код фаху має починатися з коду напряму підготовки."),

        _ => RuleResult.Ok()
    };

    private static bool StartsWithDirectionCode(Direction direction, string stateCode) =>
        stateCode.Trim().StartsWith(direction.StateCode, StringComparison.Ordinal);
}
