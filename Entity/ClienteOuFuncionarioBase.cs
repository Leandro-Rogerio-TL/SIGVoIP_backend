using System.ComponentModel.DataAnnotations;

namespace PainelIntegraTelefoniaIP.Entity;
public abstract class ClienteOuFuncionarioBase : EntityBase
{
    [Required]
    [MaxLength(250)]
    public string? NomeCompleto {get; set;}
    [Required]
    public bool TipoPJ {get; set;}
    [Required]
    [MaxLength(16)]
    public string? DocumentoNum {get; set;}
    [Required]
    [DataType(DataType.EmailAddress)]
    public string? Email {get; set;}
    [Required]
    public Endereco? Endereco {get; set;}
    public virtual Usuario? Usuario {get; set;}
    [Required]
    public bool Ativo {get; set;}
    public ClienteOuFuncionarioBase(int id) : base(id)
    {
    }
}