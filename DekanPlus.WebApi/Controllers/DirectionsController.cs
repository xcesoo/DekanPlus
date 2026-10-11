using DekanPlus.Application.Commands.Directions;
using DekanPlus.Application.DTOs;
using DekanPlus.Application.Queries.Directions;
using DekanPlus.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DekanPlus.WebApi.Controllers;

/// <summary>Напрями підготовки (державний номер, назва, кваліфікація).</summary>
[ApiController]
[Route("api/[controller]")]
public class DirectionsController(ISender sender) : ControllerBase
{
    /// <summary>Отримати список усіх напрямів підготовки (за державним номером).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<DirectionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        return Ok(await sender.Send(new GetAllDirectionsQuery(), token));
    }

    /// <summary>Створити напрям підготовки. Державний номер має бути унікальним.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateDirectionCommand command, CancellationToken token)
    {
        var id = await sender.Send(command, token);
        return StatusCode(StatusCodes.Status201Created, new CreatedResponse(id));
    }

    /// <summary>Змінити державний номер напряму підготовки. Новий номер має бути унікальним.</summary>
    [HttpPatch("state-code")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeStateCode([FromBody] ChangeDirectionStateCodeCommand command, CancellationToken token)
    {
        await sender.Send(command, token);
        return NoContent();
    }

    /// <summary>Змінити назву напряму підготовки.</summary>
    [HttpPatch("name")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeName([FromBody] ChangeDirectionNameCommand command, CancellationToken token)
    {
        await sender.Send(command, token);
        return NoContent();
    }

    /// <summary>Змінити кваліфікацію напряму підготовки.</summary>
    [HttpPatch("qualification")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeQualification([FromBody] ChangeDirectionQualificationCommand command, CancellationToken token)
    {
        await sender.Send(command, token);
        return NoContent();
    }

    /// <summary>Видалити напрям підготовки. Неможливо, якщо в ньому є фахи.</summary>
    /// <param name="id">Ідентифікатор напряму підготовки.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken token)
    {
        await sender.Send(new DeleteDirectionCommand(id), token);
        return NoContent();
    }
}
