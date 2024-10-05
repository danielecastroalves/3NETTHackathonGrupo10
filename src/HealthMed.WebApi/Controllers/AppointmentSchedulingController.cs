using System.Net;
using HealthMed.Application.Features.Appointment.CreateAppointmentScheduling;
using HealthMed.Application.Features.Appointment.GetAppointmentScheduling;
using HealthMed.Application.Features.Appointment.UpdateAppointmentScheduling;
using HealthMed.WebApi.Controllers.Comum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HealthMed.WebApi.Controllers;

/// <summary>
/// AppointmentScheduling Controller
/// </summary>
[ApiController]
[Route("v1")]
public sealed class AppointmentSchedulingController(IMediator mediator)
    : CommonController(mediator)
{
    /// <summary>
    /// CreateAppointmentSchedulingAsync - Create a new Scheduling
    /// </summary>
    /// <param name="request">CreateAppointmentSchedulingRequest</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Task</returns>
    [HttpPost("create-appointment-scheduling")]
    [Authorize]
    [SwaggerOperation(OperationId = "CreateAppointmentScheduling")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Scheduling has been created successfully")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request - Invalid input or missing required parameters")]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized - Invalid credentials or authentication token")]
    public async Task<IActionResult> CreateAppointmentSchedulingAsync([FromBody] CreateAppointmentSchedulingRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        if (result.Success)
            return Ok(result);

        return StatusCode(500, result);
    }

    /// <summary>
    /// PatchAppointmentSchedulingAsync - Update a Scheduling
    /// </summary>
    /// <param name="request">UpdateAppointmentSchedulingRequest</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Task</returns>
    [HttpPatch("patch-appointment-scheduling")]
    [Authorize]
    [SwaggerOperation(OperationId = "PatchAppointmentSchedulingAsync")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Scheduling has been updated successfully")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request - Invalid input or missing required parameters")]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized - Invalid credentials or authentication token")]
    public async Task<IActionResult> PatchAppointmentSchedulingAsync([FromBody] UpdateAppointmentSchedulingRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        if (result.Success)
            return Ok(result);

        return StatusCode(500, result);
    }

    /// <summary>
    /// GetAppointmentSchedulingAsync - Get all Scheduling by Date
    /// </summary>
    /// <param name="crmNumber">CRMNumber</param>
    /// <param name="date">Date</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Task</returns>
    [HttpGet("get-appointment-scheduling")]
    [Authorize]
    [SwaggerOperation(OperationId = "GetAppointmentSchedulingAsync")]
    [SwaggerResponse((int)HttpStatusCode.OK, "The appointment was found successfully")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request - Invalid input or missing required parameters")]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized - Invalid credentials or authentication token")]
    public async Task<IActionResult> GetAppointmentSchedulingAsync(string crmNumber, DateTime date, CancellationToken cancellationToken)
    {
        var request = new GetAppointmentSchedulingRequest { CRMNumber = crmNumber, Date = date };
        var result = await _mediator.Send(request, cancellationToken);

        if (result.Success)
            return Ok(result);

        return StatusCode(500, result);
    }
}
