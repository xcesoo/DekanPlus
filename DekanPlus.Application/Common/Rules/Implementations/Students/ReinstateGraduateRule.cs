using DekanPlus.Application.Common.Rules.Interfaces;
using DekanPlus.Domain.Enums;

namespace DekanPlus.Application.Common.Rules.Implementations.Students;

public class ReinstateGraduateRule : IBusinessRule
{
    public RuleResult Apply(IRuleContext context) => context switch
    {
        ReinstateStudentContext { Student.Expulsion.Reason: ExpulsionReason.Graduation }
            => RuleResult.Failed("Випускника поновити неможливо."),

        _ => RuleResult.Ok()
    };
}
