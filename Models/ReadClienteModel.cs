using PainelIntegraTelefoniaIP.Entity;
using PainelIntegraTelefoniaIP.Models.Interfaces;

namespace PainelIntegraTelefoniaIP.Models;

public class ReadClienteModel : IMeusDtos
{
    public string? nomeCompleto {get; set;}
    public string? documentoNum{get; set;}
    public string? email {get; set;}
    public bool ativo {get; set;}
    public Endereco? endereco {get; set;}
}