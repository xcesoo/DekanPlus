using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Interfaces;
using DekanPlus.Domain.Interfaces.Repositories;
using DekanPlus.Domain.ValueObjects;
using MediatR;

namespace DekanPlus.Application.Commands.Students;

/// <summary>Змінити прізвище, ім'я та по батькові студента.</summary>
/// <param name="Id">Ідентифікатор студента.</param>
/// <param name="LastName">Прізвище.</param>
/// <param name="FirstName">Ім'я.</param>
/// <param name="MiddleName">По батькові (необов'язково).</param>
public readonly record struct ChangeStudentFullNameCommand(
    Guid Id,
    string LastName,
    string FirstName,
    string? MiddleName) : IRequest;

public class ChangeStudentFullNameCommandHandler(
    IStudentRepository studentRepository,
    IRuleEngine ruleEngine,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeStudentFullNameCommand>
{
    public async Task Handle(ChangeStudentFullNameCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Студента не знайдено.");

        var fullName = FullName.Create(request.LastName, request.FirstName, request.MiddleName);

        var activeStudentExists = await studentRepository.ExistsActiveAsync(
            fullName, student.BirthDate, student.Id, cancellationToken);

        var context = new ChangeStudentFullNameContext(activeStudentExists);
        var validationResult = ruleEngine.Verify(context);
        if (!validationResult.IsSuccess)
            throw new InvalidOperationException(validationResult.ErrorMessage);

        student.ChangeFullName(fullName);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
