using System.Text.Json;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Features.CreateAppointmentScheduling
{
    public class CreateAppointmentSchedulingHandler : IRequestHandler<CreateAppointmentSchedulingRequest, CreateAppointmentSchedulingOutput>
    {
        private readonly IRepository<AppointmentSchedulingEntity> _schedulingRepository;
        private readonly ILogger<CreateAppointmentSchedulingHandler> _logger;

        public CreateAppointmentSchedulingHandler(IRepository<AppointmentSchedulingEntity> repository, ILogger<CreateAppointmentSchedulingHandler> logger)
        {
            _schedulingRepository = repository;
            _logger = logger;
        }

        public async Task<CreateAppointmentSchedulingOutput> Handle(CreateAppointmentSchedulingRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var schedulingList = await _schedulingRepository.GetListByFilterAsync(x => x.CRMNumber == request.CRMNumber, cancellationToken);
                if (!schedulingList.Any())
                    return await CreateSchedulingAsync(request, cancellationToken);

                var dateAlreadyScheduled = schedulingList.FirstOrDefault(x => x.Date >= request.Date && x.SchedulingDuration <= request.Date);
                if (dateAlreadyScheduled == null)
                    return await CreateSchedulingAsync(request, cancellationToken);

                return new CreateAppointmentSchedulingOutput { Success = false, Description = "Appointment date already scheduled" };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"CreateAppointmentScheduling | Error to add scheduling | Payload: {JsonSerializer.Serialize(ex)}");
                return new CreateAppointmentSchedulingOutput { Success = false, Description = "Error when adding an appointment schedule" };
            }
        }

        private async Task<CreateAppointmentSchedulingOutput> CreateSchedulingAsync(CreateAppointmentSchedulingRequest request, CancellationToken cancellationToken)
        {
            var adapted = request.Adapt<AppointmentSchedulingEntity>();
            adapted.SchedulingDuration = request.Date.AddMinutes(request.DurationInMinutes);

            await _schedulingRepository.AddAsync(adapted, cancellationToken);

            _logger.LogInformation($"CreateAppointmentScheduling | Scheduling has been added successfully | Payload: {JsonSerializer.Serialize(adapted)}");

            return new CreateAppointmentSchedulingOutput { Success = true, Description = "Scheduling has been added successfully" };
        }
    }
}
