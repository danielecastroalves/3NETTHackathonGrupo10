using MediatR;

namespace HealthMed.Application.Features.Appointment.CreateAppointmentScheduling
{
    public class CreateAppointmentSchedulingRequest : IRequest<CreateAppointmentSchedulingOutput>
    {
        public string CRMNumber { get; set; } = null!;
        public DateTime Date { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
