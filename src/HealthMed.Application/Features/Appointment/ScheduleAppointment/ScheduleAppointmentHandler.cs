using System.Text.Json;
using HealthMed.Application.Common.Repositories;
using HealthMed.Application.Common.Service;
using HealthMed.Application.Features.Doctor.GetDoctor;
using HealthMed.Application.Features.Pacient.GetPacient;
using HealthMed.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.Appointment.ScheduleAppointment;

public class ScheduleAppointmentHandler
(
    IRepository<AppointmentSchedulingEntity> schedulingRepository,
    ILogger<ScheduleAppointmentHandler> logger,
    EmailService emailService,
    IMediator mediator
) : IRequestHandler<ScheduleAppointmentRequest, ScheduleAppointmentOutput>
{
    public async Task<ScheduleAppointmentOutput> Handle(ScheduleAppointmentRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var scheduling = await schedulingRepository.GetByIdAsync(request.IdSchedule, cancellationToken);
            if (scheduling is null)
                return new ScheduleAppointmentOutput { Success = false, Description = "Appointment not found" };

            if (scheduling.PatientCPF != null)
                return new ScheduleAppointmentOutput { Success = false, Description = "Appointment Already Scheduled" };

            scheduling.PatientCPF = request.PatientCPF;
            await schedulingRepository.UpdateAppointmentAsync(x => x.Id == request.IdSchedule, scheduling, cancellationToken);

            logger.LogInformation($"CreateAppointmentScheduling | Scheduling has been updated successfully | Payload: {JsonSerializer.Serialize(scheduling)}");

            var doctorRequest = new GetDoctorRequest { CRM = scheduling.CRMNumber.ToString() };
            var doctorResponse = await mediator.Send(doctorRequest, cancellationToken);

            var patientRequest = new GetPacientRequest { CPF = scheduling.PatientCPF };
            var patientResponse = await mediator.Send(patientRequest, cancellationToken);

            await emailService.SendEmailAsync
                (doctorResponse.Email,
                "Health&Med - Nova consulta agendada", $"Olá, Dr. {doctorResponse.Nome}\r\n! Você tem uma nova consulta marcada! " +
                $"Paciente: {patientResponse.Nome}.Data e horário: {scheduling.Date.ToString("dd/MM/yyyy")} às {scheduling.Date.ToString("HH:mm")}.",
                cancellationToken);

            return new ScheduleAppointmentOutput { Success = true, Description = "Scheduling has been updated successfully" };
        }
        catch (Exception ex)
        {
            logger.LogInformation($"CreateAppointmentScheduling | Error to update scheduling | Payload: {JsonSerializer.Serialize(ex)}");

            return new ScheduleAppointmentOutput { Success = false, Description = "Error when adding an appointment schedule" };
        }
    }
}
