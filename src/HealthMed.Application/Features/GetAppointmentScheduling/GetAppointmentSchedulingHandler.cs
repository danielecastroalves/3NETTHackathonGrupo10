using HealthMed.Application.Features.CreateAppointmentScheduling;
using System.Text.Json;
using MediatR;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.GetAppointmentScheduling
{
    public class GetAppointmentSchedulingHandler : IRequestHandler<GetAppointmentSchedulingRequest, GetAppointmentSchedulingOutput>
    {
        private readonly IRepository<AppointmentSchedulingEntity> _schedulingRepository;
        private readonly ILogger<CreateAppointmentSchedulingHandler> _logger;

        public GetAppointmentSchedulingHandler(IRepository<AppointmentSchedulingEntity> repository, ILogger<CreateAppointmentSchedulingHandler> logger)
        {
            _schedulingRepository = repository;
            _logger = logger;
        }

        public async Task<GetAppointmentSchedulingOutput> Handle(GetAppointmentSchedulingRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var schedulingList = await _schedulingRepository.GetListByFilterAsync(x => x.CRMNumber == request.CRMNumber && x.Date >= request.Date, cancellationToken);
                if (!schedulingList.Any())
                    return new GetAppointmentSchedulingOutput { Success = false, Description = "No date available" };

                return new GetAppointmentSchedulingOutput { Success = true, Description = "Avaliable dates", AppointmentSchedulingList = schedulingList.ToList() };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"GetAppointmentSchedulingRequest | Error to get scheduling | Payload: {JsonSerializer.Serialize(ex)}");
                return new GetAppointmentSchedulingOutput { Success = false, Description = "Error when searching for appointment schedule" };
            }
        }
    }
}
