using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Groups;

public readonly record struct DeleteGroupCommand(Guid Id) : IRequest;

public class DeleteGroupCommandHandler(
    IGroupRepository groupRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteGroupCommand>
{
    public async Task Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await groupRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Групу не знайдено.");

        await groupRepository.DeleteAsync(group, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
