using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Implementations.Students;
using DekanPlus.Domain.Enums;
using DekanPlus.Tests.TestHelpers;

namespace DekanPlus.Tests.Application.Rules;

public class ExpelRulesTests
{
    private readonly ExpulsionDateRule _dateRule = new(FixedTimeProvider.AtToday());
    private readonly GraduationPeriodRule _graduationRule = new(new StudentRulesOptions());

    /// <summary>Дата відрахування не може бути в майбутньому відносно сьогоднішнього дня.</summary>
    [Fact]
    public void ExpulsionDate_InFuture_IsRejected()
    {
        var context = new ExpelStudentContext(TestFactory.CreateStudent(2022), new DateOnly(2026, 10, 8), ExpulsionReason.OwnReason);

        var result = _dateRule.Apply(context);

        Assert.False(result.IsSuccess);
        Assert.Contains("майбутньому", result.ErrorMessage);
    }

    /// <summary>Не можна відрахувати раніше 1 вересня року вступу, а сам цей день і сьогодні допустимі.</summary>
    [Theory]
    [InlineData("2022-08-31", false)]
    [InlineData("2022-09-01", true)]
    [InlineData("2026-10-07", true)]
    public void ExpulsionDate_BeforeEnrollment_IsRejected(string date, bool expected)
    {
        var context = new ExpelStudentContext(TestFactory.CreateStudent(2022), DateOnly.Parse(date), ExpulsionReason.OwnReason);

        Assert.Equal(expected, _dateRule.Apply(context).IsSuccess);
    }

    /// <summary>
    /// Відрахування у зв'язку із закінченням ВНЗ можливе з 1 червня року (вступ + 3): день раніше - помилка,
    /// з 1 червня - дозволено.
    /// </summary>
    [Theory]
    [InlineData("2024-05-31", false)]
    [InlineData("2024-06-01", true)]
    public void Graduation_BeforeMinimumStudyPeriod_IsRejected(string date, bool expected)
    {
        var context = new ExpelStudentContext(TestFactory.CreateStudent(2021), DateOnly.Parse(date), ExpulsionReason.Graduation);

        Assert.Equal(expected, _graduationRule.Apply(context).IsSuccess);
    }

    /// <summary>Строк навчання перевіряється лише для випуску: екстернат та інші причини відрахування не обмежені.</summary>
    [Fact]
    public void Graduation_ExternshipAndOtherReasons_AreNotRestricted()
    {
        var externship = new ExpelStudentContext(
            TestFactory.CreateStudent(2026, StudyForm.Externship), new DateOnly(2026, 10, 1), ExpulsionReason.Graduation);
        var academicFailure = new ExpelStudentContext(
            TestFactory.CreateStudent(2026), new DateOnly(2026, 10, 1), ExpulsionReason.AcademicFailure);

        Assert.True(_graduationRule.Apply(externship).IsSuccess);
        Assert.True(_graduationRule.Apply(academicFailure).IsSuccess);
    }
}
