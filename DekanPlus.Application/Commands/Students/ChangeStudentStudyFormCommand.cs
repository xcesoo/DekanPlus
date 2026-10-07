using DekanPlus.Domain.Enums;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Students;

public readonly record struct ChangeStudentStudyFormCommand(Guid Id, StudyForm StudyForm) : IRequest;

public class ChangeStudentStudyFormCommandHandler(
    IStudentRepository studentRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeStudentStudyFormCommand>
{
    public async Task Handle(ChangeStudentStudyFormCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Студента не знайдено.");

        student.ChangeStudyForm(request.StudyForm);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
