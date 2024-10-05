using System.Collections.Generic;

namespace HealthMed.Application.Features.GetAvailableDoctors
{
    public class GetAvailableDoctorsOutput
    {
        public bool Success { get; set; }
        public string Description { get; set; }
        public List<DoctorDto> AvailableDoctors { get; set; }
    }

    public class DoctorDto
    {
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
    }
}
