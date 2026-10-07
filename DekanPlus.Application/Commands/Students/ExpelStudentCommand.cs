using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Interfaces;
using DekanPlus.Domain.Enums;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Students;

public readonly record struct ExpelStudentCommand(Guid Id, DateOnly Date, ExpulsionReason Reason) : IRequest;

public class ExpelStudentCommandHandler(
    IStudentRepository studentRepository,
    IRuleEngine ruleEngine,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ExpelStudentCommand>
{
    public async Task Handle(ExpelStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Студента не знайдено.");

        var context = new ExpelStudentContext(student, request.Date, request.Reason);
        var validationResult = ruleEngine.Verify(context);
        if (!validationResult.IsSuccess)
            throw new InvalidOperationException(validationResult.ErrorMessage);

        student.Expel(request.Date, request.Reason);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
