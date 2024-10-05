using HealthMed.Domain.Entities;
using MediatR;

namespace HealthMed.Application.Features.Doctor.GetDoctor;

public class GetDoctorRequest : IRequest<GetDoctorResponse>
{
    public string CRM { get; set; } = null!;
}

public class GetDoctorResponse : PersonEntity;

