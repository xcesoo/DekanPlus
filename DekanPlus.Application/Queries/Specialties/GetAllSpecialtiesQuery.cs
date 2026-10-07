using DekanPlus.Application.DTOs;
using DekanPlus.Application.Extensions;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Queries.Specialties;

// Задача 4
public readonly record struct GetAllSpecialtiesQuery() : IRequest<IReadOnlyCollection<SpecialtyDto>>;

public class GetAllSpecialtiesQueryHandler(ISpecialtyRepository specialtyRepository)
    : IRequestHandler<GetAllSpecialtiesQuery, IReadOnlyCollection<SpecialtyDto>>
{
    public async Task<IReadOnlyCollection<SpecialtyDto>> Handle(GetAllSpecialtiesQuery request, CancellationToken cancellationToken)
    {
        var specialties = await specialtyRepository.GetAllAsync(cancellationToken);
        return specialties.Select(s => s.MapToDto()).ToList().AsReadOnly();
    }
}
