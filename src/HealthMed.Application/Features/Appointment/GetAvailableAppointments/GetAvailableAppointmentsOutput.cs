namespace HealthMed.Application.Features.Appointment.GetAvailableAppointments;

public class GetAvailableAppointmentsOutput
{
    public bool Success { get; set; }
    public string Description { get; set; } = null!;
    public List<AppointmentDto> AvailableAppointments { get; set; } = [];
}

public class AppointmentDto
{
    public Guid AppointmentId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string CRMNumber { get; set; } = null!;
}
