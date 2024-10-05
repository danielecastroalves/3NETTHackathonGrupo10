using System;
using System.Collections.Generic;

namespace HealthMed.Application.Features.GetAvailableAppointments
{
    public class GetAvailableAppointmentsOutput
    {
        public bool Success { get; set; }
        public string Description { get; set; }
        public List<AppointmentDto> AvailableAppointments { get; set; }
    }

    public class AppointmentDto
    {
        public Guid AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string DoctorId { get; set; }
    }
}
