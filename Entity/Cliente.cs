using System.ComponentModel.DataAnnotations;
using PainelIntegraTelefoniaIP.Entity.Interfaces;
using PainelIntegraTelefoniaIP.Models;

namespace PainelIntegraTelefoniaIP.Entity;

public class Cliente : ClienteOuFuncionarioBase
{
    public List<ContatoCliente>? ContatosDoCliente {get; set;}
    public List<Permissao>? PermissoesPadrao {get; set;}
    public Cliente(int id) : base(id)
    {
    }
}