using DekanPlus.Application.DTOs;
using DekanPlus.Application.Extensions;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Queries.Directions;

public readonly record struct GetAllDirectionsQuery() : IRequest<IReadOnlyCollection<DirectionDto>>;

public class GetAllDirectionsQueryHandler(IDirectionRepository directionRepository)
    : IRequestHandler<GetAllDirectionsQuery, IReadOnlyCollection<DirectionDto>>
{
    public async Task<IReadOnlyCollection<DirectionDto>> Handle(GetAllDirectionsQuery request, CancellationToken cancellationToken)
    {
        var directions = await directionRepository.GetAllAsync(cancellationToken);
        return directions.Select(d => d.MapToDto()).ToList().AsReadOnly();
    }
}
