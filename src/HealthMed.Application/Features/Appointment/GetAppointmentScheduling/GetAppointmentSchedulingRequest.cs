using MediatR;

namespace HealthMed.Application.Features.Appointment.GetAppointmentScheduling
{
    public class GetAppointmentSchedulingRequest : IRequest<GetAppointmentSchedulingOutput>
    {
        public string CRMNumber { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}
