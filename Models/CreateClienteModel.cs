using System.ComponentModel.DataAnnotations;
using PainelIntegraTelefoniaIP.Entity;
using PainelIntegraTelefoniaIP.Models.Interfaces;

namespace PainelIntegraTelefoniaIP.Models;

public class CreateClienteModel : IMeusDtos
{
    [Required]
    [Length(7, 300)]
    public string? nomeCompleto {get; set;}
    [Required]
    public bool TipoPJ {get; set;}
    [Required]
    [Length(11, 14)]
    public string? documentoNum{get; set;}
    [DataType(DataType.EmailAddress)]
    public string? email {get; set;}
    public bool ativo {get; set;}
    public Endereco? endereco {get; set;}
    public CreateClienteModel()
    {
        this.ativo = true;
    }
}