using System.Text.Json;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.Appointment.CreateAppointmentScheduling;

public class CreateAppointmentSchedulingHandler
(
    IRepository<AppointmentSchedulingEntity> repository,
    ILogger<CreateAppointmentSchedulingHandler> logger
) : IRequestHandler<CreateAppointmentSchedulingRequest, CreateAppointmentSchedulingOutput>
{
    public async Task<CreateAppointmentSchedulingOutput> Handle
    (
        CreateAppointmentSchedulingRequest request,
        CancellationToken cancellationToken
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var schedulingList = await repository
                .GetListByFilterAsync(x => x.CRMNumber == request.CRMNumber, cancellationToken);

            if (!schedulingList.Any())
                return await CreateSchedulingAsync(request, cancellationToken);

            var dateAlreadyScheduled = schedulingList
                .FirstOrDefault(x => x.Date >= request.Date && x.SchedulingDuration <= request.Date);

            if (dateAlreadyScheduled == null)
                return await CreateSchedulingAsync(request, cancellationToken);

            return new CreateAppointmentSchedulingOutput
            {
                Success = false,
                Description = "Appointment date already scheduled"
            };
        }
        catch (Exception ex)
        {
            logger.LogError(
                "CreateAppointmentScheduling | Error to add scheduling | Payload: {ex} }",
                JsonSerializer.Serialize(ex));

            return new CreateAppointmentSchedulingOutput
            {
                Success = false,
                Description = "Error when adding an appointment schedule"
            };
        }
    }

    private async Task<CreateAppointmentSchedulingOutput> CreateSchedulingAsync
    (
        CreateAppointmentSchedulingRequest request,
        CancellationToken cancellationToken
    )
    {
        var adapted = request.Adapt<AppointmentSchedulingEntity>();
        adapted.SchedulingDuration = request.Date.AddMinutes(request.DurationInMinutes);

        await repository.AddAsync(adapted, cancellationToken);

        logger.LogInformation(
            "CreateAppointmentScheduling | Scheduling has been added successfully | Payload: {Appointment}",
            JsonSerializer.Serialize(adapted));

        return new CreateAppointmentSchedulingOutput { Success = true, Description = "Scheduling has been added successfully" };
    }
}
