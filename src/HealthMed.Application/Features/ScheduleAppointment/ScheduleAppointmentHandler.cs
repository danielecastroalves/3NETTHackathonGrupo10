using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HealthMed.Application.Common.Repositories;
using HealthMed.Application.Features.UpdateAppointmentScheduling;
using HealthMed.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using HealthMed.Application.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HealthMed.Application.Features.Doctor.GetDoctor;
using HealthMed.Application.Features.Pacient.GetPacient;

namespace HealthMed.Application.Features.ScheduleAppointment
{



    public class ScheduleAppointmentHandler : IRequestHandler<ScheduleAppointmentRequest, ScheduleAppointmentOutput>
    {
        private readonly EmailService _emailService;
        private readonly IRepository<AppointmentSchedulingEntity> _schedulingRepository;
        private readonly ILogger<ScheduleAppointmentHandler> _logger;
        private readonly IMediator _mediator;



        public ScheduleAppointmentHandler(IRepository<AppointmentSchedulingEntity> schedulingRepository, ILogger<ScheduleAppointmentHandler> logger, EmailService emailService, IMediator mediator)
        {
            _schedulingRepository = schedulingRepository;
            _logger = logger;
            _emailService = emailService;
            _mediator = mediator;
        }

        public async Task<ScheduleAppointmentOutput> Handle(ScheduleAppointmentRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var scheduling = await _schedulingRepository.GetByIdAsync(request.IdSchedule, cancellationToken);
                if (scheduling is null)
                    return new ScheduleAppointmentOutput { Success = false, Description = "Appointment not found" };

                if (scheduling.PatientCPF != null)
                    return new ScheduleAppointmentOutput { Success = false, Description = "Appointment Already Scheduled" };


                scheduling.PatientCPF = request.PatientCPF;
                await _schedulingRepository.UpdateAppointmentAsync(x => x.Id == request.IdSchedule, scheduling, cancellationToken);

                _logger.LogInformation($"CreateAppointmentScheduling | Scheduling has been updated successfully | Payload: {JsonSerializer.Serialize(scheduling)}");

                var doctorRequest = new GetDoctorRequest { CRM = scheduling.CRMNumber.ToString()};
                var doctorResponse = await _mediator.Send(doctorRequest, cancellationToken);

                var patientRequest = new GetPacientRequest { CPF = scheduling.PatientCPF };
                var patientResponse = await _mediator.Send(patientRequest, cancellationToken);

                await _emailService.SendEmailAsync(doctorResponse.Email, "Health&Med - Nova consulta agendada",$"Olá, Dr. {doctorResponse.Name}\r\n! Você tem uma nova consulta marcada! Paciente: {patientResponse.Name}.Data e horário: { scheduling.Date.ToString("dd/MM/yyyy")} às { scheduling.Date.ToString("HH:mm")}.");

                return new ScheduleAppointmentOutput { Success = true, Description = "Scheduling has been updated successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"CreateAppointmentScheduling | Error to update scheduling | Payload: {JsonSerializer.Serialize(ex)}");
                return new ScheduleAppointmentOutput { Success = false, Description = "Error when adding an appointment schedule" };
            }
        }
    }
}

