using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Implementations.Students;
using DekanPlus.Domain.Enums;
using DekanPlus.Tests.TestHelpers;

namespace DekanPlus.Tests.Application.Rules;

public class ReinstateRulesTests
{
    /// <summary>Випускника (відрахованого у зв'язку із закінченням ВНЗ) поновити не можна, студента з іншої причини - можна.</summary>
    [Fact]
    public void ReinstateGraduate_OnlyGraduatesAreRejected()
    {
        var graduate = TestFactory.CreateExpelledStudent(new DateOnly(2025, 6, 30), ExpulsionReason.Graduation);
        var dropout = TestFactory.CreateExpelledStudent(new DateOnly(2025, 6, 30), ExpulsionReason.OwnReason);

        var rule = new ReinstateGraduateRule();

        Assert.False(rule.Apply(new ReinstateStudentContext(graduate, false)).IsSuccess);
        Assert.True(rule.Apply(new ReinstateStudentContext(dropout, false)).IsSuccess);
    }

    /// <summary>Поновлення можливе протягом трьох років від дати відрахування: рівно три роки - ще можна, день потому - ні.</summary>
    [Theory]
    [InlineData("2023-10-07", true)]
    [InlineData("2023-10-06", false)]
    public void ReinstatementWindow_ExpiresAfterConfiguredYears(string expulsionDate, bool expected)
    {
        var student = TestFactory.CreateExpelledStudent(DateOnly.Parse(expulsionDate), ExpulsionReason.OwnReason, 2020);
        var rule = new ReinstatementWindowRule(FixedTimeProvider.AtToday(), new StudentRulesOptions());

        Assert.Equal(expected, rule.Apply(new ReinstateStudentContext(student, false)).IsSuccess);
    }

    /// <summary>
    /// Для студента, який не відрахований, правила поновлення нічого не вирішують:
    /// помилку «студента не відраховано» повертає домен.
    /// </summary>
    [Fact]
    public void ReinstateRules_NotExpelledStudent_AreSkipped()
    {
        var student = TestFactory.CreateStudent();
        var context = new ReinstateStudentContext(student, false);

        Assert.True(new ReinstateGraduateRule().Apply(context).IsSuccess);
        Assert.True(new ReinstatementWindowRule(FixedTimeProvider.AtToday(), new StudentRulesOptions()).Apply(context).IsSuccess);
    }
}
