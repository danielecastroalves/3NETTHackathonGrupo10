using HealthMed.Domain.Entities;

namespace HealthMed.Application.Features.GetAppointmentScheduling
{
    public class GetAppointmentSchedulingOutput
    {
        public bool Success { get; set; }
        public string Description { get; set; }
        public List<AppointmentSchedulingEntity> AppointmentSchedulingList { get; set; }
    }
}
