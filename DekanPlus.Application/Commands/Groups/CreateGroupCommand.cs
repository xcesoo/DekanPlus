using DekanPlus.Application.Exceptions;
using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Groups;

public readonly record struct CreateGroupCommand(string Name, Guid SpecialtyId) : IRequest<Guid>;

public class CreateGroupCommandHandler(
    IGroupRepository groupRepository,
    ISpecialtyRepository specialtyRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateGroupCommand, Guid>
{
    public async Task<Guid> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        _ = await specialtyRepository.GetByIdAsync(request.SpecialtyId, cancellationToken)
            ?? throw new KeyNotFoundException("Фах не знайдено.");

        var group = Group.Create(request.Name, request.SpecialtyId);

        var exist = await groupRepository.GetByNameAsync(group.Name, cancellationToken);
        if (exist is not null)
            throw new DuplicateValueException("Група з такою назвою вже існує.");

        await groupRepository.AddAsync(group, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return group.Id;
    }
}
