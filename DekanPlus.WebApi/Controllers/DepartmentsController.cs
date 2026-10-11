using DekanPlus.Application.Commands.Departments;
using DekanPlus.Application.DTOs;
using DekanPlus.Application.Queries.Departments;
using DekanPlus.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DekanPlus.WebApi.Controllers;

/// <summary>Кафедри (випускаючі кафедри фахів).</summary>
[ApiController]
[Route("api/[controller]")]
public class DepartmentsController(ISender sender) : ControllerBase
{
    /// <summary>Отримати список усіх кафедр (за абеткою).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<DepartmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        return Ok(await sender.Send(new GetAllDepartmentsQuery(), token));
    }

    /// <summary>Створити кафедру. Назва має бути унікальною.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentCommand command, CancellationToken token)
    {
        var id = await sender.Send(command, token);
        return StatusCode(StatusCodes.Status201Created, new CreatedResponse(id));
    }

    /// <summary>Змінити назву кафедри. Нова назва має бути унікальною.</summary>
    [HttpPatch("name")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeName([FromBody] ChangeDepartmentNameCommand command, CancellationToken token)
    {
        await sender.Send(command, token);
        return NoContent();
    }

    /// <summary>Видалити кафедру. Неможливо, якщо за нею закріплені фахи.</summary>
    /// <param name="id">Ідентифікатор кафедри.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken token)
    {
        await sender.Send(new DeleteDepartmentCommand(id), token);
        return NoContent();
    }
}
