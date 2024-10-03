using HealthMed.Domain.Entities;
using MediatR;

namespace HealthMed.Application.Features.Client.UpdateClient;

public class UpdateClientRequest : ClientRequestBase, IRequest<ClienteEntity?>
{
    public Guid Id { get; set; }
}
