using DekanPlus.Application.Common;
using DekanPlus.Application.DTOs;
using DekanPlus.Application.Extensions;
using DekanPlus.Domain.Enums;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Queries.Students;

// Задача 2
public readonly record struct GetExpelledStudentsQuery(
    ExpulsionReason? Reason = null,
    DateOnly? From = null,
    DateOnly? To = null,
    Guid? GroupId = null) : IRequest<IReadOnlyCollection<ExpelledStudentsByReasonDto>>;

public class GetExpelledStudentsQueryHandler(IStudentRepository studentRepository)
    : IRequestHandler<GetExpelledStudentsQuery, IReadOnlyCollection<ExpelledStudentsByReasonDto>>
{
    public async Task<IReadOnlyCollection<ExpelledStudentsByReasonDto>> Handle(GetExpelledStudentsQuery request, CancellationToken cancellationToken)
    {
        if (request.From is { } from && request.To is { } to && from > to)
            throw new ArgumentException("Початкова дата не може бути пізнішою за кінцеву.");

        var students = await studentRepository.GetExpelledAsync(
            request.Reason, request.From, request.To, request.GroupId, cancellationToken);

        return students
            .GroupBy(s => s.Expulsion!.Reason)
            .OrderBy(g => g.Key)
            .Select(g => new ExpelledStudentsByReasonDto(
                Reason: g.Key,
                Count: g.Count(),
                Students: g
                    .OrderByDescending(s => s.Expulsion!.Date)
                    .ThenBy(s => s.FullName.LastName, UkrainianComparer.Instance)
                    .Select(s => s.MapToDto())
                    .ToList()
                    .AsReadOnly()))
            .ToList()
            .AsReadOnly();
    }
}
