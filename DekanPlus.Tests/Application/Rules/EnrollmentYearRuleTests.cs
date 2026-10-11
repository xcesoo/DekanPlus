using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Implementations.Students;
using DekanPlus.Tests.TestHelpers;

namespace DekanPlus.Tests.Application.Rules;

public class EnrollmentYearRuleTests
{
    private readonly EnrollmentYearRule _rule = new(FixedTimeProvider.AtToday(), new StudentRulesOptions());

    private static EnrollStudentContext Context(int year) => new(new DateOnly(2006, 5, 15), year, false);

    /// <summary>Рік вступу з майбутнього відхиляється, поточний рік допустимий.</summary>
    [Theory]
    [InlineData(2026, true)]
    [InlineData(2027, false)]
    public void Apply_FutureYear_IsRejected(int year, bool expected)
    {
        Assert.Equal(expected, _rule.Apply(Context(year)).IsSuccess);
    }

    /// <summary>Рік вступу не може бути меншим за мінімальний із налаштувань (1900).</summary>
    [Theory]
    [InlineData(1900, true)]
    [InlineData(1899, false)]
    public void Apply_YearBelowMinimum_IsRejected(int year, bool expected)
    {
        Assert.Equal(expected, _rule.Apply(Context(year)).IsSuccess);
    }
}
