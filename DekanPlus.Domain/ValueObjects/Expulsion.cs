using DekanPlus.Domain.Enums;

namespace DekanPlus.Domain.ValueObjects;

public sealed record Expulsion
{
    public DateOnly Date { get; private init; }
    public ExpulsionReason Reason { get; private init; }

    private Expulsion() { }

    private Expulsion(DateOnly date, ExpulsionReason reason)
    {
        Date = date;
        Reason = reason;
    }

    public static Expulsion Create(DateOnly date, ExpulsionReason reason)
    {
        if (date == default)
            throw new ArgumentException("Дата відрахування не може бути порожньою.");
        if (!Enum.IsDefined(reason))
            throw new ArgumentException("Невідома причина відрахування.");

        return new Expulsion(date, reason);
    }
}
