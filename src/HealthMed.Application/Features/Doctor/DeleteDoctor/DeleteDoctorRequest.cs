using FluentValidation;
using MediatR;

namespace HealthMed.Application.Features.Doctor.DeleteDoctor;

public class DeleteDoctorRequest(Guid doctorID) : IRequest
{
    public Guid DoctorID { get; } = doctorID;
}

public class DeleteDoctorRequestValidator : AbstractValidator<DeleteDoctorRequest>
{
    public DeleteDoctorRequestValidator()
    {
        RuleFor(x => x.DoctorID).NotEmpty().NotNull();
    }
}
