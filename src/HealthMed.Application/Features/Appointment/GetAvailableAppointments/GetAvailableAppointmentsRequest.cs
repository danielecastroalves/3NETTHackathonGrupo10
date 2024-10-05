using MediatR;

namespace HealthMed.Application.Features.Appointment.GetAvailableAppointments;

public class GetAvailableAppointmentsRequest : IRequest<GetAvailableAppointmentsOutput>
{
    public string CRMNumber { get; set; } = null!;
    public DateTime Date { get; set; }
}
