using DekanPlus.Domain.Interfaces;
using DekanPlus.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DekanPlus.Infrastructure.Services;

public class RecordBookNumberGenerator(DekanPlusDbContext dbContext) : IRecordBookNumberGenerator
{
    public const string SequenceName = "record_book_number_seq";

    public async Task<string> GenerateAsync(string specialtyCode, int enrollmentYear, CancellationToken cancellationToken = default)
    {
        var sequenceNumber = await dbContext.Database
            .SqlQuery<long>($"""SELECT nextval('record_book_number_seq') AS "Value" """)
            .SingleAsync(cancellationToken);

        return $"{specialtyCode}-{enrollmentYear % 100:D2}-{sequenceNumber:D5}";
    }
}
