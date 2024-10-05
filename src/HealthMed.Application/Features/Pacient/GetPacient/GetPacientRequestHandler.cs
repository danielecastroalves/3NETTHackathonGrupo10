using System.Text.Json;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using HealthMed.Domain.Enums;

namespace HealthMed.Application.Features.Pacient.GetPacient;

public class GetPacientRequestHandler
(
    IRepository<PersonEntity> repositorio,
    ILogger<GetPacientRequestHandler> logger
) : IRequestHandler<GetPacientRequest, GetPacientResponse>
{
    public async Task<GetPacientResponse> Handle(GetPacientRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await repositorio.GetByFilterAsync(x =>
            x.CPF.Equals(request.CPF) &&
            x.Perfil == Roles.Paciente &&
            x.Ativo,
            cancellationToken);

        var response = entity.Adapt<GetPacientResponse>();

        logger.LogInformation(
           "[GetPacient] " +
           "[GetPacient has been executed successfully] " +
           "[Payload: {entity}]",
           JsonSerializer.Serialize(entity));

        return response;
    }
}
