using Microsoft.AspNetCore.Identity;
using PainelIntegraTelefoniaIP.Entity.Interfaces;

namespace PainelIntegraTelefoniaIP.Entity;

public class Usuario : IdentityUser, IEntity
{  
    public int ClienteOuFuncionarioId {get; set;}
    public virtual ClienteOuFuncionarioBase? Proprietario {get; set;}
    public List<Permissao>? Permissoes {get; set;}
}