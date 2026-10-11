using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Interfaces;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Students;

/// <summary>Поновити раніше відрахованого студента.</summary>
/// <param name="Id">Ідентифікатор студента.</param>
public readonly record struct ReinstateStudentCommand(Guid Id) : IRequest;

public class ReinstateStudentCommandHandler(
    IStudentRepository studentRepository,
    IRuleEngine ruleEngine,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ReinstateStudentCommand>
{
    public async Task Handle(ReinstateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Студента не знайдено.");

        var activeDuplicateExists = await studentRepository.ExistsActiveAsync(
            student.FullName, student.BirthDate, student.Id, cancellationToken);

        var context = new ReinstateStudentContext(student, activeDuplicateExists);
        var validationResult = ruleEngine.Verify(context);
        if (!validationResult.IsSuccess)
            throw new InvalidOperationException(validationResult.ErrorMessage);

        student.Reinstate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
