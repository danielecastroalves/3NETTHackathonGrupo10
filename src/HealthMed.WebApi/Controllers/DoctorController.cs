using System.Net;
using HealthMed.Application.Features.Doctor;
using HealthMed.Application.Features.Doctor.AddDoctor;
using HealthMed.Application.Features.Doctor.DeleteDoctor;
using HealthMed.Application.Features.Doctor.GetDoctor;
using HealthMed.Application.Features.Doctor.UpdateDoctor;
using HealthMed.WebApi.Controllers.Comum;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HealthMed.WebApi.Controllers;

/// <summary>
/// Doctor Controller
/// </summary>
[ApiController]
[Route("v1")]
public sealed class DoctorController(IMediator mediator)
    : CommonController(mediator)
{
    /// <summary>
    /// AddDoctorAsync - Create a new Doctor
    /// </summary>
    /// <param name="request">AddDoctor Request</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Task</returns>
    [HttpPost("doctor")]
    [AllowAnonymous]
    [SwaggerOperation(OperationId = "AddDoctorAsync")]
    [SwaggerResponse
    (
        (int)HttpStatusCode.Created,
        "Doctor has been created successfully"
    )]
    [SwaggerResponse
    (
        (int)HttpStatusCode.BadRequest,
        "Bad Request - Invalid input or missing required parameters"
    )]
    public async Task<IActionResult> AddDoctorAsync
    (
        AddDoctorRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(request, cancellationToken);

        return new ObjectResult(result)
        {
            StatusCode = 201
        };
    }

    /// <summary>
    /// GetDoctorAsync - Get Doctor from the Document
    /// </summary>
    /// <param name="request">GetDoctor Request</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>GetDoctorResponse</returns>
    [HttpGet("doctor")]
    [Authorize]
    [SwaggerOperation(OperationId = "GetDoctorAsync")]
    [SwaggerResponse
    (
        (int)HttpStatusCode.OK,
        "Here is the Doctor found"
    )]
    [SwaggerResponse
    (
        (int)HttpStatusCode.BadRequest,
        "Bad Request - Invalid input or missing required parameters"
    )]
    [SwaggerResponse
    (
        (int)HttpStatusCode.Unauthorized,
        "Unauthorized - Invalid credentials or authentication token"
    )]
    public async Task<IActionResult> GetDoctorAsync
    (
        [FromQuery] GetDoctorRequest request,
        CancellationToken cancellationToken
    )
    {
        var doctor = await _mediator.Send(request, cancellationToken);

        return Ok(doctor);
    }

    /// <summary>
    /// UpdateDoctorAsync - Updates a Doctor register
    /// </summary>
    /// <param name="doctorId">Doctor Id - GUID</param>
    /// <param name="request">UpdateDoctor Request</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Task</returns>
    [HttpPut("doctor/{doctorId}")]
    [Authorize]
    [SwaggerOperation(OperationId = "UpdateDoctorAsync")]
    [SwaggerResponse
    (
        (int)HttpStatusCode.NoContent,
        "Doctor has been updated successfully"
    )]
    [SwaggerResponse
    (
        (int)HttpStatusCode.BadRequest,
        "Bad Request - Invalid input or missing required parameters"
    )]
    [SwaggerResponse
    (
        (int)HttpStatusCode.Unauthorized,
        "Unauthorized - Invalid credentials or authentication token"
    )]
    public async Task<IActionResult> UpdateDoctorAsync
    (
        [FromRoute] Guid doctorId,
        [FromBody] DoctorRequestBase request,
        CancellationToken cancellationToken = default
    )
    {
        var doctor = request.Adapt<UpdateDoctorRequest>();
        doctor.Id = doctorId;

        var result = await _mediator.Send(doctor, cancellationToken);

        return result is null ? NotFound() : NoContent();
    }

    /// <summary>
    /// DeleteDoctorAsync - Delete a Doctor register from given id
    /// </summary>
    /// <param name="doctorId">Doctor Id - GUID</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Task</returns>
    [HttpDelete("doctor/{doctorId}")]
    [Authorize]
    [SwaggerOperation(OperationId = "DeleteDoctorAsync")]
    [SwaggerResponse
    (
        (int)HttpStatusCode.OK,
        "Doctor has been deleted successfully"
    )]
    [SwaggerResponse
    (
        (int)HttpStatusCode.BadRequest,
        "Failed to delete Doctor register"
    )]
    [SwaggerResponse
    (
        (int)HttpStatusCode.Unauthorized,
        "Unauthorized - Invalid credentials or authentication token"
    )]
    public async Task<IActionResult> DeleteDoctorAsync
    (
        [FromRoute] Guid doctorId,
        CancellationToken cancellationToken = default
    )
    {
        await _mediator.Send(new DeleteDoctorRequest(doctorId), cancellationToken);

        return Ok();
    }
}
