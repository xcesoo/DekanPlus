using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Application.Common.Rules.Implementations.Students;

public class DuplicateStudentRule : IBusinessRule
{
    public RuleResult Apply(IRuleContext context) => context switch
    {
        EnrollStudentContext { ActiveStudentExists: true }
            => RuleResult.Failed("Студент з таким ПІБ та датою народження вже зарахований."),

        ChangeStudentFullNameContext { ActiveStudentExists: true }
            => RuleResult.Failed("Студент з таким ПІБ та датою народження вже навчається."),

        ReinstateStudentContext { ActiveDuplicateExists: true }
            => RuleResult.Failed("Неможливо поновити: студент з таким ПІБ та датою народження вже навчається."),

        _ => RuleResult.Ok()
    };
}
