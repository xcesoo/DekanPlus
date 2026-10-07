using DekanPlus.Application.Common;
using DekanPlus.Application.DTOs;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Queries.Groups;

// Задача 3
public readonly record struct GetGroupStudentCountsQuery() : IRequest<IReadOnlyCollection<GroupStudentCountDto>>;

public class GetGroupStudentCountsQueryHandler(IGroupRepository groupRepository)
    : IRequestHandler<GetGroupStudentCountsQuery, IReadOnlyCollection<GroupStudentCountDto>>
{
    public async Task<IReadOnlyCollection<GroupStudentCountDto>> Handle(GetGroupStudentCountsQuery request, CancellationToken cancellationToken)
    {
        var groups = await groupRepository.GetAllWithStudentsAsync(cancellationToken);

        return groups
            .OrderBy(g => g.Name, UkrainianComparer.Instance)
            .Select(g => new GroupStudentCountDto(
                GroupId: g.Id,
                GroupName: g.Name,
                SpecialtyName: g.Specialty.Name,
                ActiveStudents: g.Students.Count(s => !s.IsExpelled),
                ExpelledStudents: g.Students.Count(s => s.IsExpelled)))
            .ToList()
            .AsReadOnly();
    }
}
