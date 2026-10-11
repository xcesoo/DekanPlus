using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Implementations.Students;
using DekanPlus.Application.Common.Rules.Interfaces;
using DekanPlus.Domain.Enums;
using DekanPlus.Tests.TestHelpers;

namespace DekanPlus.Tests.Application.Rules;

public class DuplicateStudentRuleTests
{
    private readonly DuplicateStudentRule _rule = new();

    public static TheoryData<IRuleContext> ContextsWithDuplicate => new()
    {
        new EnrollStudentContext(new DateOnly(2006, 5, 15), 2024, ActiveStudentExists: true),
        new ChangeStudentFullNameContext(ActiveStudentExists: true),
        new ReinstateStudentContext(
            TestFactory.CreateExpelledStudent(new DateOnly(2025, 6, 1), ExpulsionReason.OwnReason),
            ActiveDuplicateExists: true)
    };

    public static TheoryData<IRuleContext> ContextsWithoutDuplicate => new()
    {
        new EnrollStudentContext(new DateOnly(2006, 5, 15), 2024, ActiveStudentExists: false),
        new ChangeStudentFullNameContext(ActiveStudentExists: false),
        new ReinstateStudentContext(
            TestFactory.CreateExpelledStudent(new DateOnly(2025, 6, 1), ExpulsionReason.OwnReason),
            ActiveDuplicateExists: false)
    };

    /// <summary>Активний студент з тим самим ПІБ і датою народження блокує зарахування, зміну ПІБ та поновлення.</summary>
    [Theory]
    [MemberData(nameof(ContextsWithDuplicate))]
    public void Apply_ActiveDuplicateExists_IsRejected(IRuleContext context)
    {
        Assert.False(_rule.Apply(context).IsSuccess);
    }

    /// <summary>Без дубліката правило пропускає всі три операції.</summary>
    [Theory]
    [MemberData(nameof(ContextsWithoutDuplicate))]
    public void Apply_NoDuplicate_IsAllowed(IRuleContext context)
    {
        Assert.True(_rule.Apply(context).IsSuccess);
    }
}
