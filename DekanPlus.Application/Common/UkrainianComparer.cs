namespace DekanPlus.Application.Common;

internal sealed class UkrainianComparer : IComparer<string?>
{
    private const string Alphabet = "абвгґдеєжзиіїйклмнопрстуфхцчшщьюя";

    public static readonly UkrainianComparer Instance = new();

    public int Compare(string? x, string? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;

        int i = 0, j = 0;
        while (true)
        {
            while (i < x.Length && IsApostrophe(x[i])) i++;
            while (j < y.Length && IsApostrophe(y[j])) j++;

            if (i >= x.Length || j >= y.Length)
                break;

            var result = Weight(x[i]).CompareTo(Weight(y[j]));
            if (result != 0) return result;

            i++;
            j++;
        }

        var tail = (x.Length - i).CompareTo(y.Length - j);
        return tail != 0 ? tail : string.CompareOrdinal(x, y);
    }

    private static bool IsApostrophe(char c) => c is '\'' or '\u2019' or '\u02BC';

    private static int Weight(char c)
    {
        var lower = char.ToLowerInvariant(c);
        var index = Alphabet.IndexOf(lower);
        return index >= 0 ? 10_000 + index : lower;
    }
}
