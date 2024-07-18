using System.ComponentModel.DataAnnotations;
using PainelIntegraTelefoniaIP.Entity.Interfaces;

namespace PainelIntegraTelefoniaIP.Entity;

public abstract class EntityBase : IEntity
{
    [Key]
    [Required]
    public int Id { get; set; }

    public EntityBase(int id)
    {
        Id = id;
    }
}