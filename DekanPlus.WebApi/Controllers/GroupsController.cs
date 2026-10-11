using DekanPlus.Application.Commands.Groups;
using DekanPlus.Application.DTOs;
using DekanPlus.Application.Queries.Groups;
using DekanPlus.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DekanPlus.WebApi.Controllers;

/// <summary>Навчальні групи.</summary>
[ApiController]
[Route("api/[controller]")]
public class GroupsController(ISender sender) : ControllerBase
{
    /// <summary>Отримати список усіх груп (за назвою).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<GroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        return Ok(await sender.Send(new GetAllGroupsQuery(), token));
    }

    /// <summary>
    /// Задача 1: видати список студентів за вказаною групою (за абеткою) разом з даними про фах, напрям та кафедру.
    /// </summary>
    /// <param name="id">Ідентифікатор групи.</param>
    /// <param name="includeExpelled">Чи включати відрахованих студентів. За замовчуванням — лише ті, що навчаються.</param>
    [HttpGet("{id:guid}/students")]
    [ProducesResponseType(typeof(GroupStudentsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudents(Guid id, [FromQuery] bool includeExpelled, CancellationToken token)
    {
        return Ok(await sender.Send(new GetStudentsByGroupQuery(id, includeExpelled), token));
    }

    /// <summary>
    /// Задача 3: підрахувати кількість студентів у кожній групі (окремо ті, що навчаються, і відраховані).
    /// Групи без студентів теж входять до списку.
    /// </summary>
    [HttpGet("student-counts")]
    [ProducesResponseType(typeof(IReadOnlyCollection<GroupStudentCountDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentCounts(CancellationToken token)
    {
        return Ok(await sender.Send(new GetGroupStudentCountsQuery(), token));
    }

    /// <summary>Створити групу. Назва має бути унікальною.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateGroupCommand command, CancellationToken token)
    {
        var id = await sender.Send(command, token);
        return StatusCode(StatusCodes.Status201Created, new CreatedResponse(id));
    }

    /// <summary>Змінити назву групи. Нова назва має бути унікальною.</summary>
    [HttpPatch("name")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeName([FromBody] ChangeGroupNameCommand command, CancellationToken token)
    {
        await sender.Send(command, token);
        return NoContent();
    }

    /// <summary>Видалити групу. Неможливо, якщо в ній є студенти (у т.ч. відраховані).</summary>
    /// <param name="id">Ідентифікатор групи.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken token)
    {
        await sender.Send(new DeleteGroupCommand(id), token);
        return NoContent();
    }
}
