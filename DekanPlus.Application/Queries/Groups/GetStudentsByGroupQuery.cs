using DekanPlus.Application.Common;
using DekanPlus.Application.DTOs;
using DekanPlus.Application.Extensions;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Queries.Groups;

// Задача 1
public readonly record struct GetStudentsByGroupQuery(Guid GroupId, bool IncludeExpelled = false) : IRequest<GroupStudentsDto>;

public class GetStudentsByGroupQueryHandler(IGroupRepository groupRepository)
    : IRequestHandler<GetStudentsByGroupQuery, GroupStudentsDto>
{
    public async Task<GroupStudentsDto> Handle(GetStudentsByGroupQuery request, CancellationToken cancellationToken)
    {
        var group = await groupRepository.GetWithStudentsAsync(request.GroupId, request.IncludeExpelled, cancellationToken)
            ?? throw new KeyNotFoundException("Групу не знайдено.");

        var students = group.Students
            .OrderBy(s => s.FullName.LastName, UkrainianComparer.Instance)
            .ThenBy(s => s.FullName.FirstName, UkrainianComparer.Instance)
            .ThenBy(s => s.FullName.MiddleName, UkrainianComparer.Instance)
            .Select(s => s.MapToGroupStudentDto())
            .ToList()
            .AsReadOnly();

        return new GroupStudentsDto(
            GroupId: group.Id,
            GroupName: group.Name,
            SpecialtyStateCode: group.Specialty.StateCode,
            SpecialtyName: group.Specialty.Name,
            DirectionName: group.Specialty.Direction.Name,
            DepartmentName: group.Specialty.Department.Name,
            StudentsCount: students.Count,
            Students: students);
    }
}
