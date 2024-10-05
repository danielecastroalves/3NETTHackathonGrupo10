namespace HealthMed.Domain.Entities
{
    public class AppointmentSchedulingEntity : Entity
    {
        public string CRMNumber { get; set; } = null!;
        public DateTime Date { get; set; }
        public DateTime SchedulingDuration { get; set; }
        public string? PatientCPF { get; set; }
    }
}
