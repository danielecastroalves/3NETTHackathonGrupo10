namespace HealthMed.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; private set; }
    public DateTime DataInsercao { get; private set; }
    public DateTime DataAtualizacao { get; private set; }
    public bool Ativo { get; private set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
        Ativo = true;
        SetDataInsercao();
        SetDataAtualizacao();
    }

    public void SetDataInsercao()
    {
        DataInsercao = DateTime.Now;
        SetDataAtualizacao();
    }

    public void SetDataAtualizacao()
    {
        DataAtualizacao = DateTime.Now;
    }

    public void SetUsuarioAtivo()
    {
        Ativo = true;
    }

    public void SetUsuarioInativo()
    {
        Ativo = false;
    }
}
