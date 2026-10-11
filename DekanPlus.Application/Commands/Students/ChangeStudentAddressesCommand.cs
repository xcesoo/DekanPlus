using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Students;

/// <summary>Змінити домашню адресу та адресу проживання студента.</summary>
/// <param name="Id">Ідентифікатор студента.</param>
/// <param name="HomeAddress">Домашня адреса.</param>
/// <param name="ResidenceAddress">Адреса проживання (необов'язково; якщо не вказана, збігається з домашньою).</param>
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
