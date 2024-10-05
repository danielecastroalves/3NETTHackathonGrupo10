using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.Appointment.GetAvailableAppointments;

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
                a.CRMNumber == request.CRM &&
                a.Date.Date.Day == request.Dia &&
                a.Date.Date.Month == request.Mes &&
                a.Date.Date.Year == request.Ano &&
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
