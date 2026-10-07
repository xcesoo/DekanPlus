using DekanPlus.Application.Common.Rules.Interfaces;
using DekanPlus.Domain.Enums;

namespace DekanPlus.Application.Common.Rules.Implementations.Students;

public class GraduationPeriodRule(StudentRulesOptions options) : IBusinessRule
{
    public RuleResult Apply(IRuleContext context)
    {
        if (context is not ExpelStudentContext { Reason: ExpulsionReason.Graduation } ctx)
            return RuleResult.Ok();

        if (ctx.Student.StudyForm == StudyForm.Externship)
            return RuleResult.Ok();

        var earliestGraduation = new DateOnly(ctx.Student.EnrollmentYear, 9, 1)
            .AddYears(options.MinStudyYearsForGraduation);

        if (ctx.Date < earliestGraduation)
            return RuleResult.Failed("Строк навчання ще не завершено.");

        return RuleResult.Ok();
    }
}
