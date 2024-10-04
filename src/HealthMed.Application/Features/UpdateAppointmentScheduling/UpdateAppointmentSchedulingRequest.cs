using HealthMed.Application.Features.CreateAppointmentScheduling;
using MediatR;

namespace HealthMed.Application.Features.UpdateAppointmentScheduling
{
    public class UpdateAppointmentSchedulingRequest : IRequest<UpdateAppointmentSchedulingOutput>
    {
        public Guid IdSchedule{ get; set; }
        public DateTime Date { get; set; }
        public int DurationInMinutes { get; set;}
    }
}
