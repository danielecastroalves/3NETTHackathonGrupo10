using System.Text.Json;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.Doctor.DeleteDoctor;

public class DeleteDoctorRequestHandler
(
    IRepository<DoctorEntity> repositorio,
    ILogger<DeleteDoctorRequestHandler> logger
) : IRequestHandler<DeleteDoctorRequest>
{
    public async Task<Unit> Handle(DeleteDoctorRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await repositorio.GetByFilterAsync(x => x.Id == request.DoctorID, cancellationToken);

        entity.SetUsuarioInativo();

        await repositorio.UpdateAsync(x => x.Id == entity.Id, entity, cancellationToken);

        logger.LogInformation(
          "[DeleteDoctor] " +
          "[Doctor has been deleted successfully] " +
          "[Payload: {entity}]",
          JsonSerializer.Serialize(entity));

        return Unit.Value;
    }
}
