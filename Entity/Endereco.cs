using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PainelIntegraTelefoniaIP.Entity;

[ComplexType]
public class Endereco
{
    [Required]
    public string Cep {get; set;}
    [Required]
    public string Logradouro{get; set;}
    [Required]
    public string Numero{get; set;}
    public string? Complemento{get; set;}
    [Required]
    public string Bairro{get; set;}
    [Required]
    public string Cidade{get; set;}
    [Required]
    public string Uf{get; set;}
    public string Pais{get; set;}
    public Endereco(string cep, string logradouro, string numero, string bairro, string cidade, string uf, 
                    string? complemento, string pais = "Brasil") 
    {
        this.Cep = cep;
        this.Logradouro = logradouro;
        this.Numero = numero;
        this.Complemento = complemento;
        this.Bairro = bairro;
        this.Cidade = cidade;
        this.Uf = uf;
        this.Pais = pais;
    }
}