namespace DekanPlus.Domain.Interfaces;

public interface IRecordBookNumberGenerator
{
    Task<string> GenerateAsync(string specialtyCode, int enrollmentYear, CancellationToken cancellationToken = default);
}
