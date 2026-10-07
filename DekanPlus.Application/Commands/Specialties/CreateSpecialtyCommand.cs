using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Interfaces;
using DekanPlus.Application.Exceptions;
using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Specialties;

public readonly record struct CreateSpecialtyCommand(
    string StateCode,
    string Name,
    string Qualification,
    Guid DirectionId,
    Guid DepartmentId) : IRequest<Guid>;

public class CreateSpecialtyCommandHandler(
    ISpecialtyRepository specialtyRepository,
    IDirectionRepository directionRepository,
    IDepartmentRepository departmentRepository,
    IRuleEngine ruleEngine,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateSpecialtyCommand, Guid>
{
    public async Task<Guid> Handle(CreateSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var direction = await directionRepository.GetByIdAsync(request.DirectionId, cancellationToken)
            ?? throw new KeyNotFoundException("Напрям підготовки не знайдено.");

        _ = await departmentRepository.GetByIdAsync(request.DepartmentId, cancellationToken)
            ?? throw new KeyNotFoundException("Кафедру не знайдено.");

        var specialty = Specialty.Create(
            request.StateCode, request.Name, request.Qualification,
            request.DirectionId, request.DepartmentId);

        var context = new CreateSpecialtyContext(direction, specialty.StateCode);
        var validationResult = ruleEngine.Verify(context);
        if (!validationResult.IsSuccess)
            throw new InvalidOperationException(validationResult.ErrorMessage);

        var exist = await specialtyRepository.GetByStateCodeAsync(specialty.StateCode, cancellationToken);
        if (exist is not null)
            throw new DuplicateValueException("Фах з таким державним номером вже існує.");

        await specialtyRepository.AddAsync(specialty, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return specialty.Id;
    }
}
