using System.Text.Json;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.Pacient.DeletePacient;

public class DeletePacientRequestHandler
(
    IRepository<ClienteEntity> repositorio,
    ILogger<DeletePacientRequestHandler> logger
) : IRequestHandler<DeletePacientRequest>
{
    public async Task<Unit> Handle(DeletePacientRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await repositorio.GetByFilterAsync(x => x.Id == request.PacientID, cancellationToken);

        entity.SetUsuarioInativo();

        await repositorio.UpdateAsync(x => x.Id == entity.Id, entity, cancellationToken);

        logger.LogInformation(
          "[DeletePacient] " +
          "[Pacient has been deleted successfully] " +
          "[Payload: {entity}]",
          JsonSerializer.Serialize(entity));

        return Unit.Value;
    }
}
