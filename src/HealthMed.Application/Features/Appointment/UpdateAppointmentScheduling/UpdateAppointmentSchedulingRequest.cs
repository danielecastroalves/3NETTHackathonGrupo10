using MediatR;

namespace HealthMed.Application.Features.Appointment.UpdateAppointmentScheduling
{
    public class UpdateAppointmentSchedulingRequest : IRequest<UpdateAppointmentSchedulingOutput>
    {
        public Guid IdSchedule { get; set; }
        public DateTime Date { get; set; }
        public int DurationInMinutes { get; set; }
        public string PatientCPF { get; set; } = null!;
    }
}
