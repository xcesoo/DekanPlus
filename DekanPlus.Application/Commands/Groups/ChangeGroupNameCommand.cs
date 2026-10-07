using DekanPlus.Application.Exceptions;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Groups;

public readonly record struct ChangeGroupNameCommand(Guid Id, string Name) : IRequest;

public class ChangeGroupNameCommandHandler(
    IGroupRepository groupRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeGroupNameCommand>
{
    public async Task Handle(ChangeGroupNameCommand request, CancellationToken cancellationToken)
    {
        var group = await groupRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Групу не знайдено.");

        group.ChangeName(request.Name);

        var exist = await groupRepository.GetByNameAsync(group.Name, cancellationToken);
        if (exist is not null && exist.Id != group.Id)
            throw new DuplicateValueException("Група з такою назвою вже існує.");

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
