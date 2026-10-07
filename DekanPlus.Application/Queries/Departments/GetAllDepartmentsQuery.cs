using DekanPlus.Application.Common;
using DekanPlus.Application.DTOs;
using DekanPlus.Application.Extensions;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Queries.Departments;

public readonly record struct GetAllDepartmentsQuery() : IRequest<IReadOnlyCollection<DepartmentDto>>;

public class GetAllDepartmentsQueryHandler(IDepartmentRepository departmentRepository)
    : IRequestHandler<GetAllDepartmentsQuery, IReadOnlyCollection<DepartmentDto>>
{
    public async Task<IReadOnlyCollection<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var departments = await departmentRepository.GetAllAsync(cancellationToken);
        return departments
            .OrderBy(d => d.Name, UkrainianComparer.Instance)
            .Select(d => d.MapToDto())
            .ToList()
            .AsReadOnly();
    }
}
