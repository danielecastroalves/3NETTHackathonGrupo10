using HealthMed.Domain.Entities;
using MediatR;

namespace HealthMed.Application.Features.Pacient.UpdatePacient;

public class UpdatePacientRequest : PacientRequestBase, IRequest<ClienteEntity?>
{
    public Guid Id { get; set; }
}
