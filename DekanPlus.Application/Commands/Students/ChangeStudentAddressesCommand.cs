using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Students;

public readonly record struct ChangeStudentAddressesCommand(
    Guid Id,
    string HomeAddress,
    string? ResidenceAddress) : IRequest;

public class ChangeStudentAddressesCommandHandler(
    IStudentRepository studentRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeStudentAddressesCommand>
{
    public async Task Handle(ChangeStudentAddressesCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Студента не знайдено.");

        student.ChangeAddresses(request.HomeAddress, request.ResidenceAddress);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
