using System.Text.Json;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.Client.UpdateClient;

public class UpdateClientRequestHandler : IRequestHandler<UpdateClientRequest, ClienteEntity?>
{
    private readonly IRepository<ClienteEntity> _repositorio;
    private readonly ILogger<UpdateClientRequestHandler> _logger;

    public UpdateClientRequestHandler
    (
        IRepository<ClienteEntity> repositorio,
        ILogger<UpdateClientRequestHandler> logger
    )
    {
        _repositorio = repositorio;
        _logger = logger;
    }

    public async Task<ClienteEntity?> Handle(UpdateClientRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await _repositorio.GetByFilterAsync(x =>
            x.Id == request.Id,
            cancellationToken);

        entity.SetUsuarioAtivo();

        entity = request.Adapt<ClienteEntity>();

        await _repositorio.UpdateAsync(x => x.Id == entity.Id, entity, cancellationToken);

        _logger.LogInformation(
            "[UpdateClient] " +
            "[Client has been updated successfully] " +
            "[Payload: {entity}]",
            JsonSerializer.Serialize(entity));

        return entity;
    }
}
