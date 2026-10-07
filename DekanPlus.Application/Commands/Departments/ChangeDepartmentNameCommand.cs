using DekanPlus.Application.Exceptions;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Departments;

public readonly record struct ChangeDepartmentNameCommand(Guid Id, string Name) : IRequest;

public class ChangeDepartmentNameCommandHandler(
    IDepartmentRepository departmentRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeDepartmentNameCommand>
{
    public async Task Handle(ChangeDepartmentNameCommand request, CancellationToken cancellationToken)
    {
        var department = await departmentRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Кафедру не знайдено.");

        department.ChangeName(request.Name);

        var exist = await departmentRepository.GetByNameAsync(department.Name, cancellationToken);
        if (exist is not null && exist.Id != department.Id)
            throw new DuplicateValueException("Кафедра з такою назвою вже існує.");

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
