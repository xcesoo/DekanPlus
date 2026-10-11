using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Implementations.Students;
using DekanPlus.Tests.TestHelpers;

namespace DekanPlus.Tests.Application.Rules;

public class StudentAgeRuleTests
{
    private readonly StudentAgeRule _rule = new(FixedTimeProvider.AtToday(), new StudentRulesOptions());

    private static EnrollStudentContext Context(DateOnly birthDate, int enrollmentYear) => new(birthDate, enrollmentYear, false);

    /// <summary>
    /// Вік рахується на 1 вересня року вступу: рівно 16 років проходить, на день менше - ні;
    /// 29 лютого у високосний рік не ламає розрахунок.
    /// </summary>
    [Theory]
    [InlineData("2008-09-01", 2024, true)]
    [InlineData("2008-09-02", 2024, false)]
    [InlineData("2008-02-29", 2024, true)]
    public void Apply_MinimumAgeIsCountedOnFirstOfSeptember(string birthDate, int enrollmentYear, bool expected)
    {
        var result = _rule.Apply(Context(DateOnly.Parse(birthDate), enrollmentYear));

        Assert.Equal(expected, result.IsSuccess);
    }

    /// <summary>Верхня межа віку: 70 років допустимо, 71 - ні (захист від помилкових дат на кшталт 1850 року).</summary>
    [Theory]
    [InlineData("1954-09-01", true)]
    [InlineData("1953-08-31", false)]
    [InlineData("1850-01-01", false)]
    public void Apply_MaximumAge_IsRejected(string birthDate, bool expected)
    {
        var result = _rule.Apply(Context(DateOnly.Parse(birthDate), 2024));

        Assert.Equal(expected, result.IsSuccess);
    }

    /// <summary>Дата народження в майбутньому відхиляється з відповідним повідомленням.</summary>
    [Fact]
    public void Apply_BirthDateInFuture_IsRejected()
    {
        var result = _rule.Apply(Context(new DateOnly(2030, 1, 1), 2024));

        Assert.False(result.IsSuccess);
        Assert.Contains("майбутньому", result.ErrorMessage);
    }

    /// <summary>Некоректний рік вступу не кидає виняток у правилі: його відхиляє правило року вступу.</summary>
    [Theory]
    [InlineData(0)]
    [InlineData(99999)]
    public void Apply_InvalidEnrollmentYear_DoesNotThrow(int year)
    {
        var result = _rule.Apply(Context(new DateOnly(2000, 1, 1), year));

        Assert.True(result.IsSuccess);
    }
}
