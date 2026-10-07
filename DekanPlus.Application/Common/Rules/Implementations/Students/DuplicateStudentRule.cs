using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Application.Common.Rules.Implementations.Students;

public class DuplicateStudentRule : IBusinessRule
{
    public RuleResult Apply(IRuleContext context) => context switch
    {
        EnrollStudentContext { ActiveStudentExists: true }
            => RuleResult.Failed("Студент з таким ПІБ та датою народження вже зарахований."),

        _ => RuleResult.Ok()
    };
}
