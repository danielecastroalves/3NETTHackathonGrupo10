using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.GetAvailableDoctors
{
    public class GetAvailableDoctorsHandler : IRequestHandler<GetAvailableDoctorsRequest, GetAvailableDoctorsOutput>
    {
        private readonly IRepository<DoctorEntity> _doctorRepository;
        private readonly IRepository<AppointmentSchedulingEntity> _schedulingRepository;
        private readonly ILogger<GetAvailableDoctorsHandler> _logger;

        public GetAvailableDoctorsHandler(IRepository<DoctorEntity> doctorRepository, IRepository<AppointmentSchedulingEntity> schedulingRepository, ILogger<GetAvailableDoctorsHandler> logger)
        {
            _doctorRepository = doctorRepository;
            _schedulingRepository = schedulingRepository;
            _logger = logger;
        }

        public async Task<GetAvailableDoctorsOutput> Handle(GetAvailableDoctorsRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var allDoctors = await _doctorRepository.GetAllAsync(cancellationToken);
                var availableDoctors = new List<DoctorDto>();

                foreach (var doctor in allDoctors)
                {
                    var appointments = await _schedulingRepository.GetAsync(a => a.DoctorId == doctor.Id && a.Date == request.Date && a.PatientCPF == null, cancellationToken);
                    if (appointments.Any())
                    {
                        availableDoctors.Add(new DoctorDto
                        {
                            DoctorId = doctor.Id,
                            DoctorName = doctor.Name
                        });
                    }
                }

                return new GetAvailableDoctorsOutput
                {
                    Success = true,
                    Description = "Available doctors retrieved successfully",
                    AvailableDoctors = availableDoctors
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving available doctors: {ex.Message}");
                return new GetAvailableDoctorsOutput
                {
                    Success = false,
                    Description = "Error retrieving available doctors",
                    AvailableDoctors = new List<DoctorDto>()
                };
            }
        }
    }
}
