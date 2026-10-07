using DekanPlus.Application.Common;
using DekanPlus.Application.DTOs;
using DekanPlus.Application.Extensions;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Queries.Students;

public readonly record struct GetAllStudentsQuery() : IRequest<IReadOnlyCollection<StudentDto>>;

public class GetAllStudentsQueryHandler(IStudentRepository studentRepository)
    : IRequestHandler<GetAllStudentsQuery, IReadOnlyCollection<StudentDto>>
{
    public async Task<IReadOnlyCollection<StudentDto>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        var students = await studentRepository.GetAllAsync(cancellationToken);
        return students
            .OrderBy(s => s.FullName.LastName, UkrainianComparer.Instance)
            .ThenBy(s => s.FullName.FirstName, UkrainianComparer.Instance)
            .Select(s => s.MapToDto())
            .ToList()
            .AsReadOnly();
    }
}
