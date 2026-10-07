using System.Globalization;

namespace DekanPlus.Application.Common;

internal static class UkrainianComparer
{
    public static readonly StringComparer Instance =
        StringComparer.Create(CultureInfo.GetCultureInfo("uk-UA"), ignoreCase: true);
}
