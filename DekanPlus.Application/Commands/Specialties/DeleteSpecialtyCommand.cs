using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Specialties;

public readonly record struct DeleteSpecialtyCommand(Guid Id) : IRequest;

public class DeleteSpecialtyCommandHandler(
    ISpecialtyRepository specialtyRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteSpecialtyCommand>
{
    public async Task Handle(DeleteSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var specialty = await specialtyRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Фах не знайдено.");

        await specialtyRepository.DeleteAsync(specialty, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
