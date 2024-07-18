using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PainelIntegraTelefoniaIP.Entity;

namespace PainelIntegraTelefoniaIP.Data.Context;

public class PainelDbContext : IdentityDbContext<Usuario>
{
    public PainelDbContext(DbContextOptions<PainelDbContext> options) : base(options)
    {
    }
    public DbSet<ClienteOuFuncionarioBase> ClienteOuFuncionario{ get; set; }
    public DbSet<Usuario> Usuarios {get; set;}
    public DbSet<Permissao> Permissoes {get; set;}
    public DbSet<Funcionario> Funcionarios {get; set;}
    public DbSet<Telefone> Telefones {get; set;}
    public DbSet<Departamento> Departamentos {get; set;}
    public DbSet<Cliente> Clientes {get; set;}
    public DbSet<ContatoCliente> ContatosCliente {get; set;}
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasSequence<int>("ClienteOuFuncionarioId");
        builder.Entity<ClienteOuFuncionarioBase>()
                .UseTpcMappingStrategy()
                .Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR [ClienteOuFuncionarioId]");
        builder.Entity<ClienteOuFuncionarioBase>().ComplexProperty(c => c.Endereco);
        base.OnModelCreating(builder);
    }
}