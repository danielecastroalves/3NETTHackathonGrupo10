using System.Text.Json;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.Pacient.AddPacient;

public class AddPacientRequestHandler
(
    IRepository<ClienteEntity> repositorio,
    ILogger<AddPacientRequestHandler> logger
) : IRequestHandler<AddPacientRequest>
{
    public async Task<Unit> Handle(AddPacientRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = request.Adapt<ClienteEntity>();

        await repositorio.AddAsync(entity, cancellationToken);

        logger.LogInformation(
            "[AddPacient] " +
            "[Pacient has been added successfully] " +
            "[Payload: {entity}]",
            JsonSerializer.Serialize(entity));

        return Unit.Value;
    }
}
