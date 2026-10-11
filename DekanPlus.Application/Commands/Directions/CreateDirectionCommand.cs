using DekanPlus.Application.Exceptions;
using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Directions;

/// <summary>Створити напрям підготовки.</summary>
/// <param name="StateCode">Державний номер (код), наприклад F3.</param>
/// <param name="Name">Назва.</param>
/// <param name="Qualification">Кваліфікація, що присуджується випускнику.</param>
public readonly record struct CreateDirectionCommand(
    string StateCode,
    string Name,
    string Qualification) : IRequest<Guid>;

public class CreateDirectionCommandHandler(
    IDirectionRepository directionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateDirectionCommand, Guid>
{
    public async Task<Guid> Handle(CreateDirectionCommand request, CancellationToken cancellationToken)
    {
        var direction = Direction.Create(request.StateCode, request.Name, request.Qualification);

        var exist = await directionRepository.GetByStateCodeAsync(direction.StateCode, cancellationToken);
        if (exist is not null)
            throw new DuplicateValueException("Напрям підготовки з таким державним номером вже існує.");

        await directionRepository.AddAsync(direction, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return direction.Id;
    }
}
