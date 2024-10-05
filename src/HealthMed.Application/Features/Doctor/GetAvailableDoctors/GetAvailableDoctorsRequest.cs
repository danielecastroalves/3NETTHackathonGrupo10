using MediatR;

namespace HealthMed.Application.Features.Doctor.GetAvailableDoctors
{
    public class GetAvailableDoctorsRequest : IRequest<GetAvailableDoctorsOutput>
    {
        public DateTime Date { get; set; }
    }
}
