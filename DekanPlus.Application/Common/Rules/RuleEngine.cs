using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Application.Common.Rules;

public class RuleEngine(IEnumerable<IBusinessRule> rules) : IRuleEngine
{
    public RuleResult Verify(IRuleContext context)
    {
        foreach (var rule in rules)
        {
            var result = rule.Apply(context);
            if (!result.IsSuccess)
            {
                return result;
            }
        }

        return RuleResult.Ok();
    }
}
