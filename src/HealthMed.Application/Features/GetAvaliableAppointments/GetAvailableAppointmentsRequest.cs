using HealthMed.Application.Features.GetAvailableAppointments;
using MediatR;

namespace HealthMed.Application.Features.GetAvaliableAppointments;

public class GetAvailableAppointmentsRequest : IRequest<GetAvailableAppointmentsOutput>
{
    public string CRMNumber { get; set; } = null!;
    public DateTime Date { get; set; }
}
