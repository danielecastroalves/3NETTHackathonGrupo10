using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.WebApi.Controllers.Comum;

/// <summary>
/// CommonController
/// </summary>
public abstract class CommonController : ControllerBase
{
    /// <summary>
    /// Mediator to execute requests
    /// </summary>
    protected readonly IMediator _mediator;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mediator">Mediator</param>
    protected CommonController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
