using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Directions;

public readonly record struct DeleteDirectionCommand(Guid Id) : IRequest;

public class DeleteDirectionCommandHandler(
    IDirectionRepository directionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteDirectionCommand>
{
    public async Task Handle(DeleteDirectionCommand request, CancellationToken cancellationToken)
    {
        var direction = await directionRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Напрям підготовки не знайдено.");

        await directionRepository.DeleteAsync(direction, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
