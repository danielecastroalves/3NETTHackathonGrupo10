using HealthMed.Application.Common.Repositories;
using HealthMed.Application.Features.GetAvaliableAppointments;
using HealthMed.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.GetAvailableAppointments;

public class GetAvailableAppointmentsHandler
(
    IRepository<AppointmentSchedulingEntity> schedulingRepository,
    ILogger<GetAvailableAppointmentsHandler> logger
) : IRequestHandler<GetAvailableAppointmentsRequest, GetAvailableAppointmentsOutput>
{
    public async Task<GetAvailableAppointmentsOutput> Handle
    (
        GetAvailableAppointmentsRequest request,
        CancellationToken cancellationToken
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var availableAppointments = await schedulingRepository.GetAsync(a =>
                a.CRMNumber == request.CRMNumber &&
                a.Date.Date == request.Date.Date &&
                a.PatientCPF == null,
                cancellationToken);

            var appointmentDtos = availableAppointments.Select(a => new AppointmentDto
            {
                AppointmentId = a.Id,
                AppointmentDate = a.Date,
                CRMNumber = a.CRMNumber
            }).ToList();

            return new GetAvailableAppointmentsOutput
            {
                Success = true,
                Description = "Available appointments retrieved successfully",
                AvailableAppointments = appointmentDtos
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving available appointments: {Message}", ex.Message);

            return new GetAvailableAppointmentsOutput
            {
                Success = false,
                Description = "Error retrieving available appointments",
                AvailableAppointments = []
            };
        }
    }
}
