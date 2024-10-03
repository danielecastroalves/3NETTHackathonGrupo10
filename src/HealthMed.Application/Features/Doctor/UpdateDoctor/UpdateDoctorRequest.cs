using HealthMed.Domain.Entities;
using MediatR;

namespace HealthMed.Application.Features.Doctor.UpdateDoctor;

public class UpdateDoctorRequest : DoctorRequestBase, IRequest<ClienteEntity?>
{
    public Guid Id { get; set; }
}
