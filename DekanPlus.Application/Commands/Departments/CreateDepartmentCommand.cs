using DekanPlus.Application.Exceptions;
using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Departments;

public readonly record struct CreateDepartmentCommand(string Name) : IRequest<Guid>;

public class CreateDepartmentCommandHandler(
    IDepartmentRepository departmentRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateDepartmentCommand, Guid>
{
    public async Task<Guid> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = Department.Create(request.Name);

        var exist = await departmentRepository.GetByNameAsync(department.Name, cancellationToken);
        if (exist is not null)
            throw new DuplicateValueException("Кафедра з такою назвою вже існує.");

        await departmentRepository.AddAsync(department, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return department.Id;
    }
}
