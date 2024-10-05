using MediatR;

namespace HealthMed.Application.Features.GetAvailableDoctors
{
    public class GetAvailableDoctorsRequest : IRequest<GetAvailableDoctorsOutput>
    {
        public DateTime Date { get; set; }
    }
}
