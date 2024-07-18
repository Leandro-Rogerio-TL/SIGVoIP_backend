using System.ComponentModel.DataAnnotations;

namespace PainelIntegraTelefoniaIP.Entity;

public class Departamento : EntityBase
{
    [Required]
    [MaxLength(50)]
    public string? NomeDepartamento {get; set;}
    public List<Funcionario>? Funcionarios {get; set;}
    public List<Permissao>? PermissoesPadrao {get; set;}
    public Departamento(int id) : base(id)
    {
    }
}