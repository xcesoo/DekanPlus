using DekanPlus.Application.Common;
using DekanPlus.Application.DTOs;
using DekanPlus.Application.Extensions;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Queries.Groups;

public readonly record struct GetAllGroupsQuery() : IRequest<IReadOnlyCollection<GroupDto>>;

public class GetAllGroupsQueryHandler(IGroupRepository groupRepository)
    : IRequestHandler<GetAllGroupsQuery, IReadOnlyCollection<GroupDto>>
{
    public async Task<IReadOnlyCollection<GroupDto>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        var groups = await groupRepository.GetAllAsync(cancellationToken);
        return groups
            .OrderBy(g => g.Name, UkrainianComparer.Instance)
            .Select(g => g.MapToDto())
            .ToList()
            .AsReadOnly();
    }
}
