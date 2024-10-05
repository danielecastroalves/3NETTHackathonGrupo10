using MediatR;

namespace HealthMed.Application.Features.Appointment.GetAvailableAppointments;

public class GetAvailableAppointmentsRequest : IRequest<GetAvailableAppointmentsOutput>
{
    public string CRM { get; set; } = null!;
    public int Dia { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
}
