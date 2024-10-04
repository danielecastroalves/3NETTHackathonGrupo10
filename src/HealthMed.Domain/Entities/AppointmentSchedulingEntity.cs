namespace HealthMed.Domain.Entities
{
    public class AppointmentSchedulingEntity : Entity
    {
        public int CRMNumber { get; set; }
        public DateTime Date { get; set; }
        public DateTime SchedulingDuration { get; set; }
    }
}
