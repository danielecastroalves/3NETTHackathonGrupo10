using System.Net;
using HealthMed.Application.Features.GetAvailableDoctors;
using HealthMed.Application.Features.GetAvaliableAppointments;
using HealthMed.Application.Features.ScheduleAppointment;
using HealthMed.WebApi.Controllers.Comum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HealthMed.WebApi.Controllers;

/// <summary>
/// HealthMed Controller
/// </summary>
[ApiController]
[Route("v1")]
public sealed class PatientAppointmentController(IMediator mediator)
    : CommonController(mediator)
{
    /// <summary>
    /// ScheduleAppointmentAsync - Schedule a new appointment
    /// </summary>
    /// <param name="request">ScheduleAppointmentRequest</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Task</returns>
    [HttpPost("schedule-appointment")]
    [Authorize]
    [SwaggerOperation(OperationId = "ScheduleAppointment")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Appointment has been scheduled successfully")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request - Invalid input or missing required parameters")]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized - Invalid credentials or authentication token")]
    public async Task<IActionResult> ScheduleAppointmentAsync([FromBody] ScheduleAppointmentRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        if (result.Success)
            return Ok(result);

        return StatusCode(500, result);
    }

    /// <summary>
    /// GetAvailableAppointmentsAsync - Get available appointments
    /// </summary>
    /// <param name="crmNumber">CRMNumber</param>
    /// <param name="date">Date</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Task</returns>
    [HttpGet("available-appointments")]
    [Authorize]
    [SwaggerOperation(OperationId = "GetAvailableAppointments")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Available appointments retrieved successfully")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request - Invalid input or missing required parameters")]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized - Invalid credentials or authentication token")]
    public async Task<IActionResult> GetAvailableAppointmentsAsync(string crmNumber, DateTime date, CancellationToken cancellationToken)
    {
        var request = new GetAvailableAppointmentsRequest { CRMNumber = crmNumber, Date = date };
        var result = await _mediator.Send(request, cancellationToken);

        if (result.Success)
            return Ok(result);

        return StatusCode(500, result);
    }

    /// <summary>
    /// GetAvailableDoctorsAsync - Get available doctors
    /// </summary>
    /// <param name="date">Date</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Task</returns>
    [HttpGet("available-doctors")]
    [Authorize]
    [SwaggerOperation(OperationId = "GetAvailableDoctors")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Available doctors retrieved successfully")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request - Invalid input or missing required parameters")]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized - Invalid credentials or authentication token")]
    public async Task<IActionResult> GetAvailableDoctorsAsync(DateTime date, CancellationToken cancellationToken)
    {
        var request = new GetAvailableDoctorsRequest { Date = date };
        var result = await _mediator.Send(request, cancellationToken);

        if (result.Success)
            return Ok(result);

        return StatusCode(500, result);
    }
}
