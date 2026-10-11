using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Application.Common.Rules.Implementations.Directions;

public class DirectionCodeMatchesSpecialtiesRule : IBusinessRule
{
    public RuleResult Apply(IRuleContext context) => context switch
    {
        ChangeDirectionStateCodeContext ctx
            when ctx.Direction.Specialties.Any(s => !s.StateCode.StartsWith((ctx.StateCode ?? string.Empty).Trim(), StringComparison.Ordinal))
            => RuleResult.Failed("Неможливо змінити державний номер: коди фахів цього напряму мають починатися з нового номера."),

        _ => RuleResult.Ok()
    };
}
