using DekanPlus.Application.Commands.Specialties;
using DekanPlus.Application.DTOs;
using DekanPlus.Application.Queries.Specialties;
using DekanPlus.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DekanPlus.WebApi.Controllers;

/// <summary>Фахи (спеціальності) з напрямом підготовки та випускаючою кафедрою.</summary>
[ApiController]
[Route("api/[controller]")]
public class SpecialtiesController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Задача 4: видати список фахів (державний номер, назва, кваліфікація, напрям підготовки, кафедра).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<SpecialtyDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        return Ok(await sender.Send(new GetAllSpecialtiesQuery(), token));
    }

    /// <summary>
    /// Створити фах. Державний номер має бути унікальним і починатися з коду напряму підготовки (наприклад, F3 для напряму F).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateSpecialtyCommand command, CancellationToken token)
    {
        var id = await sender.Send(command, token);
        return StatusCode(StatusCodes.Status201Created, new CreatedResponse(id));
    }

    /// <summary>
    /// Змінити державний номер фаху. Номер має бути унікальним і починатися з коду напряму підготовки фаху.
    /// </summary>
    [HttpPatch("state-code")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeStateCode([FromBody] ChangeSpecialtyStateCodeCommand command, CancellationToken token)
    {
        await sender.Send(command, token);
        return NoContent();
    }

    /// <summary>Змінити назву фаху.</summary>
    [HttpPatch("name")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeName([FromBody] ChangeSpecialtyNameCommand command, CancellationToken token)
    {
        await sender.Send(command, token);
        return NoContent();
    }

    /// <summary>Змінити кваліфікацію фаху.</summary>
    [HttpPatch("qualification")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeQualification([FromBody] ChangeSpecialtyQualificationCommand command, CancellationToken token)
    {
        await sender.Send(command, token);
        return NoContent();
    }

    /// <summary>Видалити фах. Неможливо, якщо за ним є групи.</summary>
    /// <param name="id">Ідентифікатор фаху.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken token)
    {
        await sender.Send(new DeleteSpecialtyCommand(id), token);
        return NoContent();
    }
}
