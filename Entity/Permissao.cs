using System.ComponentModel.DataAnnotations;

namespace PainelIntegraTelefoniaIP.Entity;

public class Permissao : EntityBase
{
    [Required]
    [MaxLength(100)]
    public string? Funcionalidade {get; set;}
    public List<Usuario>? Usuarios{get; set;}
    public List<Departamento>? PadraoDeAcessoPorDepartamento {get; set;}
    public List<Cliente>? PadraoDeAcessoParaCliente {get; set;}
    public Permissao(int id) : base(id)
    {
    }
}