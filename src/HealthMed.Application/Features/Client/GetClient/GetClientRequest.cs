using HealthMed.Domain.Entities;
using MediatR;

namespace HealthMed.Application.Features.Client.GetClient;

public class GetClientRequest : IRequest<GetClientResponse>
{
    public string Documento { get; set; } = null!;
}

public class GetClientResponse : ClienteEntity { }
