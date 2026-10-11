using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Interfaces;

namespace DekanPlus.Tests.Application.Rules;

public class RuleEngineTests
{
    private sealed class StubRule(RuleResult result) : IBusinessRule
    {
        public int Calls { get; private set; }

        public RuleResult Apply(IRuleContext context)
        {
            Calls++;
            return result;
        }
    }

    private sealed record StubContext : IRuleContext;

    /// <summary>Рушій зупиняється на першому правилі, що не пройшло, і повертає його повідомлення; наступні правила не викликаються.</summary>
    [Fact]
    public void Verify_StopsAtFirstFailedRule()
    {
        var first = new StubRule(RuleResult.Failed("перша помилка"));
        var second = new StubRule(RuleResult.Failed("друга помилка"));
        var engine = new RuleEngine([first, second]);

        var result = engine.Verify(new StubContext());

        Assert.False(result.IsSuccess);
        Assert.Equal("перша помилка", result.ErrorMessage);
        Assert.Equal(0, second.Calls);
    }

    /// <summary>Якщо жодне правило не заперечує, результат успішний.</summary>
    [Fact]
    public void Verify_AllRulesPass_ReturnsOk()
    {
        var engine = new RuleEngine([new StubRule(RuleResult.Ok()), new StubRule(RuleResult.Ok())]);

        Assert.True(engine.Verify(new StubContext()).IsSuccess);
    }
}
