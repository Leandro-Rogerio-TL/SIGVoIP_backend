using System.ComponentModel.DataAnnotations;
using PainelIntegraTelefoniaIP.Entity.Interfaces;

namespace PainelIntegraTelefoniaIP.Entity;

public class ContatoCliente : EntityBase
{
    [Required]
    [MaxLength(150)]
    public string? NomeCompleto {get; set;}
    [Required]
    [DataType(DataType.EmailAddress)]
    public string? Email {get; private set;}
    public List<Telefone>? Telefones {get; set;}
    [MaxLength(50)]
    public string? Departamento{get; set;}
    [MaxLength(50)]
    public string? Cargo{get; set;}
    [Required]
    public bool Ativo {get; set;}
    [Required]
    public int ClienteId {get; set;}
    public virtual Cliente? cliente{get; set;}
    public ContatoCliente (int id) : base (id)
    {
    }
}