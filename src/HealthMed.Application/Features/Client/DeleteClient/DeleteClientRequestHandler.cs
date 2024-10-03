using System.Text.Json;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.Client.DeleteClient;

public class DeleteClientRequestHandler : IRequestHandler<DeleteClientRequest>
{
    private readonly IRepository<ClienteEntity> _repositorio;
    private readonly ILogger<DeleteClientRequestHandler> _logger;

    public DeleteClientRequestHandler
    (
        IRepository<ClienteEntity> repositorio,
        ILogger<DeleteClientRequestHandler> logger
    )
    {
        _repositorio = repositorio;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteClientRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await _repositorio.GetByFilterAsync(x => x.Id == request.ClientID, cancellationToken);

        entity.SetUsuarioInativo();

        await _repositorio.UpdateAsync(x => x.Id == entity.Id, entity, cancellationToken);

        _logger.LogInformation(
          "[DeleteClient] " +
          "[Client has been deleted successfully] " +
          "[Payload: {entity}]",
          JsonSerializer.Serialize(entity));

        return Unit.Value;
    }
}
