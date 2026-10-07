using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Interfaces;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Students;

public readonly record struct TransferStudentCommand(Guid Id, Guid GroupId) : IRequest;

public class TransferStudentCommandHandler(
    IStudentRepository studentRepository,
    IGroupRepository groupRepository,
    IRuleEngine ruleEngine,
    IUnitOfWork unitOfWork)
    : IRequestHandler<TransferStudentCommand>
{
    public async Task Handle(TransferStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Студента не знайдено.");

        var group = await groupRepository.GetByIdAsync(request.GroupId, cancellationToken)
            ?? throw new KeyNotFoundException("Групу не знайдено.");

        var context = new TransferStudentContext(student, group);
        var validationResult = ruleEngine.Verify(context);
        if (!validationResult.IsSuccess)
            throw new InvalidOperationException(validationResult.ErrorMessage);

        student.TransferToGroup(group.Id);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
