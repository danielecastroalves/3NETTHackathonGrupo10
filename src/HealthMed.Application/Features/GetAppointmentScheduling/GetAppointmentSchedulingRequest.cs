using MediatR;

namespace HealthMed.Application.Features.GetAppointmentScheduling
{
    public class GetAppointmentSchedulingRequest : IRequest<GetAppointmentSchedulingOutput>
    {
        public int CRMNumber { get; set; }
        public DateTime Date { get; set; }
    }
}
