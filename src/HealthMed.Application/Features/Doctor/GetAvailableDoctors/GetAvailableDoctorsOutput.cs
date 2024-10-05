namespace HealthMed.Application.Features.Doctor.GetAvailableDoctors
{
    public class GetAvailableDoctorsOutput
    {
        public bool Success { get; set; }
        public string Description { get; set; } = null!;
        public List<DoctorDto> AvailableDoctors { get; set; } = [];
    }

    public class DoctorDto
    {
        public string DoctorCRM { get; set; } = null!;
        public string DoctorName { get; set; } = null!;
    }
}
