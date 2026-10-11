using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Directions;

/// <summary>Змінити кваліфікацію напряму підготовки.</summary>
/// <param name="Id">Ідентифікатор напряму підготовки.</param>
/// <param name="Qualification">Кваліфікація, що присуджується випускнику.</param>
public readonly record struct ChangeDirectionQualificationCommand(Guid Id, string Qualification) : IRequest;

public class ChangeDirectionQualificationCommandHandler(
    IDirectionRepository directionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeDirectionQualificationCommand>
{
    public async Task Handle(ChangeDirectionQualificationCommand request, CancellationToken cancellationToken)
    {
        var direction = await directionRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Напрям підготовки не знайдено.");

        direction.ChangeQualification(request.Qualification);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
