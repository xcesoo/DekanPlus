using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Departments;

public readonly record struct DeleteDepartmentCommand(Guid Id) : IRequest;

public class DeleteDepartmentCommandHandler(
    IDepartmentRepository departmentRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteDepartmentCommand>
{
    public async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await departmentRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Кафедру не знайдено.");

        await departmentRepository.DeleteAsync(department, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
