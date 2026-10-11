using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Implementations.Directions;
using DekanPlus.Application.Common.Rules.Implementations.Specialties;
using DekanPlus.Tests.TestHelpers;

namespace DekanPlus.Tests.Application.Rules;

public class CodeConsistencyRulesTests
{
    /// <summary>Код фаху має починатися з коду напряму підготовки (пробіли по краях ігноруються).</summary>
    [Theory]
    [InlineData("F3", true)]
    [InlineData(" F5 ", true)]
    [InlineData("G7", false)]
    public void SpecialtyCode_MustStartWithDirectionCode(string specialtyCode, bool expected)
    {
        var direction = TestFactory.CreateDirectionWithSpecialties("F");
        var rule = new SpecialtyCodeMatchesDirectionRule();

        Assert.Equal(expected, rule.Apply(new CreateSpecialtyContext(direction, specialtyCode)).IsSuccess);
        Assert.Equal(expected, rule.Apply(new ChangeSpecialtyStateCodeContext(direction, specialtyCode)).IsSuccess);
    }

    /// <summary>
    /// Змінити код напряму можна, лише якщо коди всіх його фахів починаються з нового коду;
    /// інакше порушилась би узгодженість «код фаху починається з коду напряму».
    /// </summary>
    [Theory]
    [InlineData("F", true)]
    [InlineData("F3", false)]
    [InlineData("X", false)]
    public void DirectionCode_MustStayConsistentWithSpecialties(string newCode, bool expected)
    {
        var direction = TestFactory.CreateDirectionWithSpecialties("F", "F3", "F5");
        var rule = new DirectionCodeMatchesSpecialtiesRule();

        Assert.Equal(expected, rule.Apply(new ChangeDirectionStateCodeContext(direction, newCode)).IsSuccess);
    }
}
