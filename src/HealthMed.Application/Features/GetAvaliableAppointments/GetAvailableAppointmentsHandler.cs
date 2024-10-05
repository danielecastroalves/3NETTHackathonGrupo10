using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.GetAvailableAppointments
{
    public class GetAvailableAppointmentsHandler : IRequestHandler<GetAvailableAppointmentsRequest, GetAvailableAppointmentsOutput>
    {
        private readonly IRepository<AppointmentSchedulingEntity> _schedulingRepository;
        private readonly ILogger<GetAvailableAppointmentsHandler> _logger;

        public GetAvailableAppointmentsHandler(IRepository<AppointmentSchedulingEntity> schedulingRepository, ILogger<GetAvailableAppointmentsHandler> logger)
        {
            _schedulingRepository = schedulingRepository;
            _logger = logger;
        }

        public async Task<GetAvailableAppointmentsOutput> Handle(GetAvailableAppointmentsRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var availableAppointments = await _schedulingRepository.GetAsync(
                    a => a.DoctorId == request.DoctorId && a.Date.Date == request.Date.Date && a.PatientCPF == null,
                    cancellationToken);

                var appointmentDtos = availableAppointments.Select(a => new AppointmentDto
                {
                    AppointmentId = a.Id,
                    AppointmentDate = a.Date,
                    DoctorId = a.DoctorId
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
                _logger.LogError($"Error retrieving available appointments: {ex.Message}");
                return new GetAvailableAppointmentsOutput
                {
                    Success = false,
                    Description = "Error retrieving available appointments",
                    AvailableAppointments = new List<AppointmentDto>()
                };
            }
        }
    }
}
