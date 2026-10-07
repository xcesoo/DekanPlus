using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Application.Common.Rules.Implementations.Students;

public class ReinstatementWindowRule(TimeProvider timeProvider, StudentRulesOptions options) : IBusinessRule
{
    public RuleResult Apply(IRuleContext context)
    {
        if (context is not ReinstateStudentContext { Student.Expulsion: { } expulsion })
            return RuleResult.Ok();

        var today = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);
        if (today > expulsion.Date.AddYears(options.ReinstatementWindowYears))
            return RuleResult.Failed("Строк для поновлення минув.");

        return RuleResult.Ok();
    }
}
