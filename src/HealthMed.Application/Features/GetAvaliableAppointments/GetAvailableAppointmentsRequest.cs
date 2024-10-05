using MediatR;
using System;

namespace HealthMed.Application.Features.GetAvailableAppointments
{
    public class GetAvailableAppointmentsRequest : IRequest<GetAvailableAppointmentsOutput>
    {
        public int CRMNumber { get; set; }
        public DateTime Date { get; set; }
    }
}
