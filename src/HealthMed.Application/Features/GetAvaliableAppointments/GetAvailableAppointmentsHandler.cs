using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using HealthMed.Application.Features.Doctor.GetDoctor;

namespace HealthMed.Application.Features.GetAvailableAppointments
{
    public class GetAvailableAppointmentsHandler : IRequestHandler<GetAvailableAppointmentsRequest, GetAvailableAppointmentsOutput>
    {
        private readonly IRepository<AppointmentSchedulingEntity> _schedulingRepository;
        private readonly ILogger<GetAvailableAppointmentsHandler> _logger;
        private readonly IMediator _mediator;

        public GetAvailableAppointmentsHandler(IRepository<AppointmentSchedulingEntity> schedulingRepository, ILogger<GetAvailableAppointmentsHandler> logger, IMediator mediator)
        {
            _schedulingRepository = schedulingRepository;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<GetAvailableAppointmentsOutput> Handle(GetAvailableAppointmentsRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var availableAppointments = await _schedulingRepository.GetAsync(
                    a => a.CRMNumber == request.CRMNumber && a.Date.Date == request.Date.Date && a.PatientCPF == null,
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
