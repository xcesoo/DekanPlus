using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Directions;

public readonly record struct ChangeDirectionNameCommand(Guid Id, string Name) : IRequest;

public class ChangeDirectionNameCommandHandler(
    IDirectionRepository directionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeDirectionNameCommand>
{
    public async Task Handle(ChangeDirectionNameCommand request, CancellationToken cancellationToken)
    {
        var direction = await directionRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Напрям підготовки не знайдено.");

        direction.ChangeName(request.Name);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
