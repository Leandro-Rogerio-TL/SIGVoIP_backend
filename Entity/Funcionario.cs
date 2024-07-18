using System.ComponentModel.DataAnnotations;

namespace PainelIntegraTelefoniaIP.Entity;

public class Funcionario : ClienteOuFuncionarioBase
{
    public List<Telefone>? Telefones{get; set;}
    public int DepartamentoId{get; set;}
    public virtual Departamento? Departamento{get; set;}
    public Funcionario(int id) : base(id)
    {
        base.TipoPJ = false;
    }
}