using System.Text.Json;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.Pacient.UpdatePacient;

public class UpdatePacientRequestHandler
(
    IRepository<PersonEntity> repositorio,
    ILogger<UpdatePacientRequestHandler> logger
) : IRequestHandler<UpdatePacientRequest, PersonEntity?>
{
    public async Task<PersonEntity?> Handle(UpdatePacientRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await repositorio.GetByFilterAsync(x =>
            x.Id == request.Id,
            cancellationToken);

        entity.SetUsuarioAtivo();

        entity = request.Adapt<PersonEntity>();

        await repositorio.UpdateAsync(x => x.Id == entity.Id, entity, cancellationToken);

        logger.LogInformation(
            "[UpdatePacient] " +
            "[Pacient has been updated successfully] " +
            "[Payload: {entity}]",
            JsonSerializer.Serialize(entity));

        return entity;
    }
}
