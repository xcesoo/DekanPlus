using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Application.Common.Rules.Implementations.Students;

public class ReinstatementWindowRule(TimeProvider timeProvider, StudentRulesOptions options) : IBusinessRule
{
    private DateOnly Today => DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime);

    public RuleResult Apply(IRuleContext context) => context switch
    {
        ReinstateStudentContext { Student.Expulsion: { } expulsion }
            when Today > expulsion.Date.AddYears(options.ReinstatementWindowYears)
            => RuleResult.Failed("Строк для поновлення минув."),

        _ => RuleResult.Ok()
    };
}
