using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Interfaces;
using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Enums;
using DekanPlus.Domain.Interfaces;
using DekanPlus.Domain.Interfaces.Repositories;
using DekanPlus.Domain.ValueObjects;
using MediatR;

namespace DekanPlus.Application.Commands.Students;

/// <summary>Зарахувати студента: створити його особову картку. Номер залікової книжки формується автоматично.</summary>
/// <param name="LastName">Прізвище.</param>
/// <param name="FirstName">Ім'я.</param>
/// <param name="MiddleName">По батькові (необов'язково).</param>
/// <param name="BirthDate">Дата народження.</param>
/// <param name="HomeAddress">Домашня адреса.</param>
/// <param name="ResidenceAddress">Адреса проживання (необов'язково; якщо не вказана, збігається з домашньою).</param>
/// <param name="EnrollmentYear">Рік вступу до ВНЗ.</param>
/// <param name="StudyForm">Вид навчання.</param>
/// <param name="GroupId">Ідентифікатор групи, до якої зараховується студент.</param>
public readonly record struct EnrollStudentCommand(
    string LastName,
    string FirstName,
    string? MiddleName,
    DateOnly BirthDate,
    string HomeAddress,
    string? ResidenceAddress,
    int EnrollmentYear,
    StudyForm StudyForm,
    Guid GroupId) : IRequest<Guid>;

public class EnrollStudentCommandHandler(
    IStudentRepository studentRepository,
    IGroupRepository groupRepository,
    IRuleEngine ruleEngine,
    IRecordBookNumberGenerator recordBookNumberGenerator,
    IUnitOfWork unitOfWork)
    : IRequestHandler<EnrollStudentCommand, Guid>
{
    public async Task<Guid> Handle(EnrollStudentCommand request, CancellationToken cancellationToken)
    {
        var group = await groupRepository.GetByIdAsync(request.GroupId, cancellationToken)
            ?? throw new KeyNotFoundException("Групу не знайдено.");

        var fullName = FullName.Create(request.LastName, request.FirstName, request.MiddleName);

        var activeStudentExists = await studentRepository.ExistsActiveAsync(fullName, request.BirthDate, cancellationToken: cancellationToken);

        var context = new EnrollStudentContext(request.BirthDate, request.EnrollmentYear, activeStudentExists);
        var validationResult = ruleEngine.Verify(context);
        if (!validationResult.IsSuccess)
            throw new InvalidOperationException(validationResult.ErrorMessage);

        var recordBookNumber = await recordBookNumberGenerator.GenerateAsync(
            group.Specialty.StateCode, request.EnrollmentYear, cancellationToken);

        var student = Student.Create(
            fullName,
            request.BirthDate,
            request.HomeAddress,
            request.ResidenceAddress,
            request.EnrollmentYear,
            request.StudyForm,
            recordBookNumber,
            group.Id);

        await studentRepository.AddAsync(student, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return student.Id;
    }
}
