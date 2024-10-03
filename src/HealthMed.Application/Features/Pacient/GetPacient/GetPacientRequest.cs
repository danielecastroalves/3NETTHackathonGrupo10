using HealthMed.Domain.Entities;
using MediatR;

namespace HealthMed.Application.Features.Pacient.GetPacient;

public class GetPacientRequest : IRequest<GetPacientResponse>
{
    public string CPF { get; set; } = null!;
}

public class GetPacientResponse : ClienteEntity;
