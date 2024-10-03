using System.Text.Json;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.Doctor.AddDoctor;

public class AddDoctorRequestHandler
(
    IRepository<ClienteEntity> repositorio,
    ILogger<AddDoctorRequestHandler> logger
) : IRequestHandler<AddDoctorRequest>
{
    public async Task<Unit> Handle(AddDoctorRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = request.Adapt<ClienteEntity>();

        await repositorio.AddAsync(entity, cancellationToken);

        logger.LogInformation(
            "[AddDoctor] " +
            "[Doctor has been added successfully] " +
            "[Payload: {entity}]",
            JsonSerializer.Serialize(entity));

        return Unit.Value;
    }
}
