using DekanPlus.Application.Common;

namespace DekanPlus.Tests.Application.Common;

public class UkrainianComparerTests
{
    private static string[] Sort(params string[] values) =>
        values.OrderBy(v => v, UkrainianComparer.Instance).ToArray();

    /// <summary>
    /// Літери «ґ», «є», «і» стоять на своїх місцях в алфавіті (ґ після г, є після е, і перед к),
    /// а не в кінці чи на початку, як за кодами символів.
    /// </summary>
    [Fact]
    public void Compare_FollowsUkrainianAlphabet()
    {
        var sorted = Sort("Яковенко", "Іваненко", "Київ", "Андрієнко", "Ґудзь", "Єрмоленко", "Дмитренко", "Ющенко");

        Assert.Equal(
            ["Андрієнко", "Ґудзь", "Дмитренко", "Єрмоленко", "Іваненко", "Київ", "Ющенко", "Яковенко"],
            sorted);
    }

    /// <summary>Апостроф ігнорується при порівнянні: «Лук'яненко» йде після «Лукаш», бо 'а' менше за 'я'.</summary>
    [Fact]
    public void Compare_IgnoresApostrophe()
    {
        var sorted = Sort("Лук'яненко", "Лукаш", "Лукін");

        Assert.Equal(["Лукаш", "Лукін", "Лук'яненко"], sorted);
    }
}
