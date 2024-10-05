using System.Net;
using HealthMed.Application.Features.Login;
using HealthMed.WebApi.Controllers.Comum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HealthMed.WebApi.Controllers;

/// <summary>
/// Login Controller
/// </summary>
[ApiController]
[Route("v1/login")]
public sealed class LoginController(IMediator mediator) : CommonController(mediator)
{

    /// <summary>
    /// Authenticate the user
    /// </summary>
    /// <param name="request">Login Request</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Task</returns>
    [HttpPost]
    [AllowAnonymous]
    [SwaggerResponse
    (
        (int)HttpStatusCode.OK,
        "Here is a token for authenticate the user"
    )]
    [SwaggerResponse
    (
        (int)HttpStatusCode.NotFound,
        "Not Found - Invalid Email or Password"
    )]
    [SwaggerResponse
    (
        (int)HttpStatusCode.BadRequest,
        "Bad Request - Invalid input or missing required parameters"
    )]
    public async Task<ActionResult<dynamic>> Authenticate(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var token = await _mediator.Send(request, cancellationToken);

        if (string.IsNullOrWhiteSpace(token))
        {
            return NotFound(new { message = "Invalid Email or Password" });
        }
        else
        {
            request.Password = "";

            return new
            {
                user = request,
                token
            };
        }
    }
}
