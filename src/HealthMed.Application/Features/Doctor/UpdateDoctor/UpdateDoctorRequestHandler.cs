using System.Text.Json;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.Doctor.UpdateDoctor;

public class UpdateDoctorRequestHandler
(
    IRepository<DoctorEntity> repositorio,
    ILogger<UpdateDoctorRequestHandler> logger
) : IRequestHandler<UpdateDoctorRequest, DoctorEntity?>
{
    public async Task<DoctorEntity?> Handle(UpdateDoctorRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await repositorio.GetByFilterAsync(x =>
            x.Id == request.Id,
            cancellationToken);

        entity.SetUsuarioAtivo();

        entity = request.Adapt<DoctorEntity>();

        await repositorio.UpdateAsync(x => x.Id == entity.Id, entity, cancellationToken);

        logger.LogInformation(
            "[UpdateDoctor] " +
            "[Doctor has been updated successfully] " +
            "[Payload: {entity}]",
            JsonSerializer.Serialize(entity));

        return entity;
    }
}
