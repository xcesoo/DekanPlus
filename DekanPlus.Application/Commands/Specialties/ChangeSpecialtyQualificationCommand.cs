using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Specialties;

public readonly record struct ChangeSpecialtyQualificationCommand(Guid Id, string Qualification) : IRequest;

public class ChangeSpecialtyQualificationCommandHandler(
    ISpecialtyRepository specialtyRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeSpecialtyQualificationCommand>
{
    public async Task Handle(ChangeSpecialtyQualificationCommand request, CancellationToken cancellationToken)
    {
        var specialty = await specialtyRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Фах не знайдено.");

        specialty.ChangeQualification(request.Qualification);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
