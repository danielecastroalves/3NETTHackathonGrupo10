using HealthMed.Application.Common.Repositories;
using HealthMed.Application.Features.GetAvailableDoctors;
using HealthMed.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.GetAvaliableDoctors;

public class GetAvailableDoctorsHandler
(
    IRepository<PersonEntity> doctorRepository,
    IRepository<AppointmentSchedulingEntity> schedulingRepository,
    ILogger<GetAvailableDoctorsHandler> logger
) : IRequestHandler<GetAvailableDoctorsRequest, GetAvailableDoctorsOutput>
{
    public async Task<GetAvailableDoctorsOutput> Handle
    (
        GetAvailableDoctorsRequest request,
        CancellationToken cancellationToken
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var allDoctors = await doctorRepository.GetAllAsync(cancellationToken);
            var availableDoctors = new List<DoctorDto>();

            foreach (var doctor in allDoctors)
            {
                var appointments = await schedulingRepository.GetAsync(a =>
                a.CRMNumber == doctor.CRM &&
                a.Date == request.Date &&
                a.PatientCPF == null,
                cancellationToken);

                if (appointments.Any())
                {
                    availableDoctors.Add(new DoctorDto
                    {
                        DoctorCRM = doctor.CRM,
                        DoctorName = doctor.Nome
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
            logger.LogError(ex, "Error retrieving available doctors: {Message}", ex.Message);

            return new GetAvailableDoctorsOutput
            {
                Success = false,
                Description = "Error retrieving available doctors",
                AvailableDoctors = []
            };
        }
    }
}
