using System.Text.Json;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using HealthMed.Domain.Enums;

namespace HealthMed.Application.Features.Doctor.GetDoctor;

public class GetDoctorRequestHandler
(
    IRepository<PersonEntity> repositorio,
    ILogger<GetDoctorRequestHandler> logger
) : IRequestHandler<GetDoctorRequest, GetDoctorResponse>
{
    public async Task<GetDoctorResponse> Handle(GetDoctorRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await repositorio.GetByFilterAsync(x =>
            x.CRM.Equals(request.CRM) &&
            x.Perfil == Roles.Medico &&
            x.Ativo,
            cancellationToken);

        var response = entity.Adapt<GetDoctorResponse>();

        logger.LogInformation(
           "[GetDoctor] " +
           "[GetDoctor has been executed successfully] " +
           "[Payload: {entity}]",
           JsonSerializer.Serialize(entity));

        return response;
    }
}
