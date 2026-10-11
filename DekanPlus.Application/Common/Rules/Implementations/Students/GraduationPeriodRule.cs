using DekanPlus.Application.Common.Rules.Interfaces;
using DekanPlus.Domain.Enums;

namespace DekanPlus.Application.Common.Rules.Implementations.Students;

public class GraduationPeriodRule(StudentRulesOptions options) : IBusinessRule
{
    public RuleResult Apply(IRuleContext context) => context switch
    {
        ExpelStudentContext { Reason: ExpulsionReason.Graduation, Student.StudyForm: not StudyForm.Externship } ctx
            when ctx.Date < new DateOnly(ctx.Student.EnrollmentYear + options.MinStudyYearsForGraduation, 6, 1)
            => RuleResult.Failed("Строк навчання ще не завершено."),

        _ => RuleResult.Ok()
    };
}
