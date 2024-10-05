using FluentValidation;
using HealthMed.Domain.Enums;

namespace HealthMed.Application.Features.Doctor;

public class DoctorRequestBase
{
    public string Nome { get; set; } = null!;
    public string CPF { get; set; } = null!;
    public string CRM { get; set; } = null!;
    public string Especialidade { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
}

public class DoctorRequestBaseValidator<T> : AbstractValidator<T> where T : DoctorRequestBase
{
    public DoctorRequestBaseValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().NotNull();
        RuleFor(x => x.CPF).NotEmpty().NotNull();
        RuleFor(x => x.CRM).NotEmpty().NotNull();
        RuleFor(x => x.Especialidade).NotEmpty().NotNull();
        RuleFor(x => x.Email).NotEmpty().NotNull();
        RuleFor(x => x.Senha).NotEmpty().NotNull();
    }
}
