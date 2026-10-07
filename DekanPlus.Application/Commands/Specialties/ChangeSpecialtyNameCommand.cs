using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Specialties;

public readonly record struct ChangeSpecialtyNameCommand(Guid Id, string Name) : IRequest;

public class ChangeSpecialtyNameCommandHandler(
    ISpecialtyRepository specialtyRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeSpecialtyNameCommand>
{
    public async Task Handle(ChangeSpecialtyNameCommand request, CancellationToken cancellationToken)
    {
        var specialty = await specialtyRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Фах не знайдено.");

        specialty.ChangeName(request.Name);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
