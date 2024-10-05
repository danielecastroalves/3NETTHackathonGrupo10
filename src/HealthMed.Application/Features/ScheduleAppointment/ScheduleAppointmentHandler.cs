using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HealthMed.Application.Common.Repositories;
using HealthMed.Application.Features.UpdateAppointmentScheduling;
using HealthMed.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.ScheduleAppointment
{



    public class ScheduleAppointmentHandler : IRequestHandler<ScheduleAppointmentRequest, ScheduleAppointmentOutput>
    {
        private readonly IRepository<AppointmentSchedulingEntity> _schedulingRepository;
        private readonly ILogger<ScheduleAppointmentHandler> _logger;

        public ScheduleAppointmentHandler(IRepository<AppointmentSchedulingEntity> schedulingRepository, ILogger<ScheduleAppointmentHandler> logger)
        {
            _schedulingRepository = schedulingRepository;
            _logger = logger;
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

