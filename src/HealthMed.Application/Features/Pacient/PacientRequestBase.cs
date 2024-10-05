using FluentValidation;

namespace HealthMed.Application.Features.Pacient;

public class PacientRequestBase
{
    public string Nome { get; set; } = null!;
    public string CPF { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
}

public class PacientRequestBaseValidator<T> : AbstractValidator<T> where T : PacientRequestBase
{
    public PacientRequestBaseValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().NotNull();
        RuleFor(x => x.CPF).NotEmpty().NotNull();
        RuleFor(x => x.Email).NotEmpty().NotNull();
        RuleFor(x => x.Senha).NotEmpty().NotNull();
    }
}
