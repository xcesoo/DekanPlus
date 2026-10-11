using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Interfaces;
using DekanPlus.Application.Exceptions;
using DekanPlus.Domain.Interfaces.Repositories;
using MediatR;

namespace DekanPlus.Application.Commands.Directions;

/// <summary>Змінити державний номер напряму підготовки.</summary>
/// <param name="Id">Ідентифікатор напряму підготовки.</param>
/// <param name="StateCode">Державний номер (код), наприклад F3.</param>
public readonly record struct ChangeDirectionStateCodeCommand(Guid Id, string StateCode) : IRequest;

public class ChangeDirectionStateCodeCommandHandler(
    IDirectionRepository directionRepository,
    IRuleEngine ruleEngine,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeDirectionStateCodeCommand>
{
    public async Task Handle(ChangeDirectionStateCodeCommand request, CancellationToken cancellationToken)
    {
        var direction = await directionRepository.GetWithSpecialtiesAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Напрям підготовки не знайдено.");

        var context = new ChangeDirectionStateCodeContext(direction, request.StateCode);
        var validationResult = ruleEngine.Verify(context);
        if (!validationResult.IsSuccess)
            throw new InvalidOperationException(validationResult.ErrorMessage);

        direction.ChangeStateCode(request.StateCode);

        var exist = await directionRepository.GetByStateCodeAsync(direction.StateCode, cancellationToken);
        if (exist is not null && exist.Id != direction.Id)
            throw new DuplicateValueException("Напрям підготовки з таким державним номером вже існує.");

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
