using System.Text.Json;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.Appointment.UpdateAppointmentScheduling;

public class UpdateAppointmentSchedulingHandler
(
    IRepository<AppointmentSchedulingEntity> schedulingRepository,
    ILogger<UpdateAppointmentSchedulingHandler> logger
) : IRequestHandler<UpdateAppointmentSchedulingRequest, UpdateAppointmentSchedulingOutput>
{
    public async Task<UpdateAppointmentSchedulingOutput> Handle
    (
        UpdateAppointmentSchedulingRequest request,
        CancellationToken cancellationToken
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var scheduling = await schedulingRepository.GetByIdAsync(request.IdSchedule, cancellationToken);

            if (scheduling is null)
            {
                return new UpdateAppointmentSchedulingOutput
                {
                    Success = false,
                    Description = "Appointment not found"
                };
            }

            var schedulingList = await schedulingRepository
                .GetListByFilterAsync(x => x.CRMNumber == scheduling.CRMNumber, cancellationToken);

            var initialDateAlreadyScheduled = schedulingList.FirstOrDefault(x =>
                x.Date >= request.Date &&
                x.SchedulingDuration <= request.Date);

            var newSchedulingDuration = request.Date.AddMinutes(request.DurationInMinutes);

            var finalDateAlreadyScheduled = schedulingList.FirstOrDefault(x =>
                x.Date >= newSchedulingDuration &&
                x.SchedulingDuration <= newSchedulingDuration);

            if (initialDateAlreadyScheduled is null && finalDateAlreadyScheduled is null)
                return await UpdateSchedulingAsync(request, cancellationToken);

            return new UpdateAppointmentSchedulingOutput
            {
                Success = false,
                Description = "Appointment date already scheduled"
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "CreateAppointmentScheduling | " +
                "Error to update scheduling | " +
                "Payload: {Message}",
                JsonSerializer.Serialize(ex));

            return new UpdateAppointmentSchedulingOutput
            {
                Success = false,
                Description = "Error when adding an appointment schedule"
            };
        }
    }

    private async Task<UpdateAppointmentSchedulingOutput> UpdateSchedulingAsync
    (
        UpdateAppointmentSchedulingRequest request,
        CancellationToken cancellationToken
    )
    {
        var entity = new AppointmentSchedulingEntity
        {
            Date = request.Date,
            SchedulingDuration = request.Date.AddMinutes(request.DurationInMinutes),
            PatientCPF = request.PatientCPF
        };

        await schedulingRepository.UpdateAppointmentAsync(x => x.Id == request.IdSchedule, entity, cancellationToken);

        logger.LogInformation(
            "CreateAppointmentScheduling | " +
            "Scheduling has been updated successfully | " +
            "Payload: {Message)}",
            JsonSerializer.Serialize(entity));

        return new UpdateAppointmentSchedulingOutput
        {
            Success = true,
            Description = "Scheduling has been updated successfully"
        };
    }
}
