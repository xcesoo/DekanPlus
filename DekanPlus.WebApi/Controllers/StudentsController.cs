using DekanPlus.Application.Commands.Students;
using DekanPlus.Application.DTOs;
using DekanPlus.Application.Queries.Students;
using DekanPlus.Domain.Enums;
using DekanPlus.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DekanPlus.WebApi.Controllers;

/// <summary>Особові картки студентів.</summary>
[ApiController]
[Route("api/[controller]")]
public class StudentsController(ISender sender) : ControllerBase
{
    /// <summary>Отримати список усіх студентів (за абеткою), включно з відрахованими.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<StudentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        return Ok(await sender.Send(new GetAllStudentsQuery(), token));
    }

    /// <summary>Отримати особову картку студента за ідентифікатором.</summary>
    /// <param name="id">Ідентифікатор студента.</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken token)
    {
        return Ok(await sender.Send(new GetStudentByIdQuery(id), token));
    }

    /// <summary>
    /// Задача 2: видати списки відрахованих студентів, згруповані за причиною відрахування.
    /// Усі фільтри необов'язкові.
    /// </summary>
    /// <param name="reason">Лише відраховані з цієї причини.</param>
    /// <param name="from">Відраховані не раніше цієї дати (yyyy-MM-dd).</param>
    /// <param name="to">Відраховані не пізніше цієї дати (yyyy-MM-dd).</param>
    /// <param name="groupId">Лише студенти цієї групи.</param>
    [HttpGet("expelled")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ExpelledStudentsByReasonDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetExpelled(
        [FromQuery] ExpulsionReason? reason,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        [FromQuery] Guid? groupId,
        CancellationToken token)
    {
        return Ok(await sender.Send(new GetExpelledStudentsQuery(reason, from, to, groupId), token));
    }

    /// <summary>
    /// Зарахувати студента до групи. Номер залікової книжки формується автоматично у форматі «код фаху-рік-номер».
    /// Перевіряється вік на 1 вересня року вступу, рік вступу та відсутність дубліката (ПІБ і дата народження).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Enroll([FromBody] EnrollStudentCommand command, CancellationToken token)
    {
        var id = await sender.Send(command, token);
        return CreatedAtAction(nameof(GetById), new { id }, new CreatedResponse(id));
    }

    /// <summary>Змінити ПІБ студента. Неможливо для відрахованого студента.</summary>
    [HttpPatch("full-name")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeFullName([FromBody] ChangeStudentFullNameCommand command, CancellationToken token)
    {
        await sender.Send(command, token);
        return NoContent();
    }

    /// <summary>Змінити домашню адресу та адресу проживання. Неможливо для відрахованого студента.</summary>
    [HttpPatch("addresses")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeAddresses([FromBody] ChangeStudentAddressesCommand command, CancellationToken token)
    {
        await sender.Send(command, token);
        return NoContent();
    }

    /// <summary>Змінити вид навчання. Неможливо для відрахованого студента.</summary>
    [HttpPatch("study-form")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeStudyForm([FromBody] ChangeStudentStudyFormCommand command, CancellationToken token)
    {
        await sender.Send(command, token);
        return NoContent();
    }

    /// <summary>
    /// Перевести студента до іншої групи. Нова група має відрізнятися від поточної; переведення між різними фахами
    /// заборонене, якщо це не дозволено в налаштуваннях (StudentRules).
    /// </summary>
    [HttpPatch("group")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Transfer([FromBody] TransferStudentCommand command, CancellationToken token)
    {
        await sender.Send(command, token);
        return NoContent();
    }

    /// <summary>
    /// Відрахувати студента. Дата не може бути в майбутньому і раніше вступу; відрахування «закінчення ВНЗ»
    /// можливе лише після мінімального строку навчання (для екстернату — без обмеження).
    /// </summary>
    [HttpPatch("expel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Expel([FromBody] ExpelStudentCommand command, CancellationToken token)
    {
        await sender.Send(command, token);
        return NoContent();
    }

    /// <summary>
    /// Поновити раніше відрахованого студента. Неможливо для випускників та після спливу строку поновлення.
    /// </summary>
    [HttpPatch("reinstate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Reinstate([FromBody] ReinstateStudentCommand command, CancellationToken token)
    {
        await sender.Send(command, token);
        return NoContent();
    }
}
