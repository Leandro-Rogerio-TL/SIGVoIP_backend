using System.ComponentModel.DataAnnotations;

namespace PainelIntegraTelefoniaIP.Entity;

public enum TelefoneTipo
{
    Celular = 0,
    Fixo = 1
}
public enum TelefoneUso
{
    Pessoal = 0,
    Corporativo = 1,
    Recado = 2
}
public class Telefone : EntityBase
{
    [MaxLength(3)]
    public string? Ddi {get; set;}
    [Required]
    [MaxLength(3)]
    public string? Ddd{get; set;}
    [Required]
    [MaxLength(10)]
    public string? Numero {get; set;}
    public int? Ramal {get; set;}
    public TelefoneTipo? Tipo{get; set;}
    public TelefoneUso? Utilizacao{get; set;}
    public string? NomeRecado{get; set;}
    public int? FuncionarioId{get; set;}
    public virtual Funcionario? funcionario {get; set;}
    public int? ContatoClienteId{get; set;}
    public virtual ContatoCliente? contatoCliente {get; set;}
    public Telefone(int id) : base(id)
    {
    }
}