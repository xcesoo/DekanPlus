using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Interfaces;
using DekanPlus.Application.Exceptions;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Specialties;

public readonly record struct ChangeSpecialtyStateCodeCommand(Guid Id, string StateCode) : IRequest;

public class ChangeSpecialtyStateCodeCommandHandler(
    ISpecialtyRepository specialtyRepository,
    IRuleEngine ruleEngine,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeSpecialtyStateCodeCommand>
{
    public async Task Handle(ChangeSpecialtyStateCodeCommand request, CancellationToken cancellationToken)
    {
        var specialty = await specialtyRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Фах не знайдено.");

        var context = new ChangeSpecialtyStateCodeContext(specialty.Direction, request.StateCode);
        var validationResult = ruleEngine.Verify(context);
        if (!validationResult.IsSuccess)
            throw new InvalidOperationException(validationResult.ErrorMessage);

        specialty.ChangeStateCode(request.StateCode);

        var exist = await specialtyRepository.GetByStateCodeAsync(specialty.StateCode, cancellationToken);
        if (exist is not null && exist.Id != specialty.Id)
            throw new DuplicateValueException("Фах з таким державним номером вже існує.");

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
