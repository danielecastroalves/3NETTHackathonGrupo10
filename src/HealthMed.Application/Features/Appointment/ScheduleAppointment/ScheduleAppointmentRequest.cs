using MediatR;

namespace HealthMed.Application.Features.Appointment.ScheduleAppointment
{
    public class ScheduleAppointmentRequest : IRequest<ScheduleAppointmentOutput>
    {
        public Guid IdSchedule { get; set; }
        public string PatientCPF { get; set; } = null!;
    }
}
