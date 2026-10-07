using DekanPlus.Application.Exceptions;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Directions;

public readonly record struct ChangeDirectionStateCodeCommand(Guid Id, string StateCode) : IRequest;

public class ChangeDirectionStateCodeCommandHandler(
    IDirectionRepository directionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeDirectionStateCodeCommand>
{
    public async Task Handle(ChangeDirectionStateCodeCommand request, CancellationToken cancellationToken)
    {
        var direction = await directionRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Напрям підготовки не знайдено.");

        direction.ChangeStateCode(request.StateCode);

        var exist = await directionRepository.GetByStateCodeAsync(direction.StateCode, cancellationToken);
        if (exist is not null && exist.Id != direction.Id)
            throw new DuplicateValueException("Напрям підготовки з таким державним номером вже існує.");

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
