using HealthMed.Domain.Enums;

namespace HealthMed.Domain.Entities;

public class PersonEntity : User
{
    public string Name { get; set; } = null!;
    public string CPF { get; set; } = null!;
    public string Email { get; set; } = null!;
}

public class DoctorEntity : PersonEntity
{
    public int CRM { get; set; }
    public string Specialty { get; set; } = null!;
    public string? Description { get; set; }
}

public class User : Entity
{
    public string Login { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public Roles Profiles { get; set; }
}
