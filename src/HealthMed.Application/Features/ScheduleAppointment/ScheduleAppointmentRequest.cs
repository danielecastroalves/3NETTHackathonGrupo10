using HealthMed.Application.Features.CreateAppointmentScheduling;
using MediatR;

namespace HealthMed.Application.Features.ScheduleAppointment
{
    public class ScheduleAppointmentRequest : IRequest<ScheduleAppointmentOutput>
    {
        public Guid IdSchedule { get; set; }
        public DateTime Date { get; set; }
        public int DurationInMinutes { get; set; }
        public string PatientCPF { get; set; } = null!;
    }
}
