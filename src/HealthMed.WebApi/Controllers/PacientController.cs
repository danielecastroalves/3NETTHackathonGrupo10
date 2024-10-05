using System.Net;
using HealthMed.Application.Features.Pacient;
using HealthMed.Application.Features.Pacient.AddPacient;
using HealthMed.Application.Features.Pacient.DeletePacient;
using HealthMed.Application.Features.Pacient.GetPacient;
using HealthMed.Application.Features.Pacient.UpdatePacient;
using HealthMed.WebApi.Controllers.Comum;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HealthMed.WebApi.Controllers;

/// <summary>
/// Pacient Controller
/// </summary>
[ApiController]
[Route("v1")]
public sealed class PacientController(IMediator mediator)
    : CommonController(mediator)
{
    /// <summary>
    /// AddPacientAsync - Create a new Pacient
    /// </summary>
    /// <param name="request">AddPacient Request</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Task</returns>
    [HttpPost("pacient")]
    [AllowAnonymous]
    [SwaggerOperation(OperationId = "AddPacientAsync")]
    [SwaggerResponse
    (
        (int)HttpStatusCode.Created,
        "Pacient has been created successfully"
    )]
    [SwaggerResponse
    (
        (int)HttpStatusCode.BadRequest,
        "Bad Request - Invalid input or missing required parameters"
    )]
    public async Task<IActionResult> AddPacientAsync
    (
        AddPacientRequest request,
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
    /// GetPacientAsync - Get Pacient from the Document
    /// </summary>
    /// <param name="request">GetPacient Request</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>GetPacientResponse</returns>
    [HttpGet("pacient")]
    [Authorize]
    [SwaggerOperation(OperationId = "GetPacientAsync")]
    [SwaggerResponse
    (
        (int)HttpStatusCode.OK,
        "Here is the Pacient found"
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
    public async Task<IActionResult> GetPacientAsync
    (
        [FromQuery] GetPacientRequest request,
        CancellationToken cancellationToken
    )
    {
        var pacient = await _mediator.Send(request, cancellationToken);

        return Ok(pacient);
    }

    /// <summary>
    /// UpdatePacientAsync - Updates a Pacient register
    /// </summary>
    /// <param name="pacientId">Pacient Id - GUID</param>
    /// <param name="request">UpdatePacient Request</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Task</returns>
    [HttpPut("pacient/{pacientId}")]
    [Authorize]
    [SwaggerOperation(OperationId = "UpdatePacientAsync")]
    [SwaggerResponse
    (
        (int)HttpStatusCode.NoContent,
        "Pacient has been updated successfully"
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
    public async Task<IActionResult> UpdatePacientAsync
    (
        [FromRoute] Guid pacientId,
        [FromBody] PacientRequestBase request,
        CancellationToken cancellationToken = default
    )
    {
        var pacient = request.Adapt<UpdatePacientRequest>();
        pacient.Id = pacientId;

        var result = await _mediator.Send(pacient, cancellationToken);

        return result is null ? NotFound() : NoContent();
    }

    /// <summary>
    /// DeletePacientAsync - Delete a Pacient register from given id
    /// </summary>
    /// <param name="pacientId">Pacient Id - GUID</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Task</returns>
    [HttpDelete("pacient/{pacientId}")]
    [Authorize]
    [SwaggerOperation(OperationId = "DeletePacientAsync")]
    [SwaggerResponse
    (
        (int)HttpStatusCode.OK,
        "Pacient has been deleted successfully"
    )]
    [SwaggerResponse
    (
        (int)HttpStatusCode.BadRequest,
        "Failed to delete Pacient register"
    )]
    [SwaggerResponse
    (
        (int)HttpStatusCode.Unauthorized,
        "Unauthorized - Invalid credentials or authentication token"
    )]
    public async Task<IActionResult> DeletePacientAsync
    (
        [FromRoute] Guid pacientId,
        CancellationToken cancellationToken = default
    )
    {
        await _mediator.Send(new DeletePacientRequest(pacientId), cancellationToken);

        return Ok();
    }
}
