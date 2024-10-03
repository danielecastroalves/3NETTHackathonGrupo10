using FluentValidation;
using MediatR;

namespace HealthMed.Application.Features.Pacient.DeletePacient;

public class DeletePacientRequest(Guid pacientID) : IRequest
{
    public Guid PacientID { get; } = pacientID;
}

public class DeletePacientRequestValidator : AbstractValidator<DeletePacientRequest>
{
    public DeletePacientRequestValidator()
    {
        RuleFor(x => x.PacientID).NotEmpty().NotNull();
    }
}
