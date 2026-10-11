namespace DekanPlus.Infrastructure.Services;

public sealed class ConfiguredTimeProvider : TimeProvider
{
    public ConfiguredTimeProvider(string? timeZoneId)
    {
        LocalTimeZone = !string.IsNullOrWhiteSpace(timeZoneId) && TimeZoneInfo.TryFindSystemTimeZoneById(timeZoneId, out var timeZone)
            ? timeZone
            : TimeZoneInfo.Local;
    }

    public override TimeZoneInfo LocalTimeZone { get; }
}
