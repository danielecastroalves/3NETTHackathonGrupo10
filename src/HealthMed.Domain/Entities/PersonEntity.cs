using HealthMed.Domain.Enums;

namespace HealthMed.Domain.Entities;

public class PersonEntity : User
{
    public string Nome { get; set; } = null!;
    public string CPF { get; set; } = null!;
    public string CRM { get; set; } = null!;
    public string Especialidade { get; set; } = null!;
    public string? Descrição { get; set; }
}

public class User : Entity
{
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public Roles Perfil { get; set; }
}
