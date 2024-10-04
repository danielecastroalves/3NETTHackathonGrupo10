using MediatR;

namespace HealthMed.Application.Features.CreateAppointmentScheduling
{
    public class CreateAppointmentSchedulingRequest : IRequest<CreateAppointmentSchedulingOutput>
    {
        public int CRMNumber { get; set; }
        public DateTime Date { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
