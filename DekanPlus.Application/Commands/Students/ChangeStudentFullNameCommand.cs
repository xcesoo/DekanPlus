using DekanPlus.Domain.Interfaces.Repositories;
using DekanPlus.Domain.ValueObjects;
using MediatR;

namespace DekanPlus.Application.Commands.Students;

public readonly record struct ChangeStudentFullNameCommand(
    Guid Id,
    string LastName,
    string FirstName,
    string? MiddleName) : IRequest;

public class ChangeStudentFullNameCommandHandler(
    IStudentRepository studentRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeStudentFullNameCommand>
{
    public async Task Handle(ChangeStudentFullNameCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Студента не знайдено.");

        student.ChangeFullName(FullName.Create(request.LastName, request.FirstName, request.MiddleName));
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
