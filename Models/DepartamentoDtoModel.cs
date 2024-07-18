using System.ComponentModel.DataAnnotations;
using PainelIntegraTelefoniaIP.Models.Interfaces;

namespace PainelIntegraTelefoniaIP.Models;

public class DepartamentoDtoModel : IMeusDtos
{
    [Required]
    [MaxLength(50)]
    public string? NomeDepartamento {get; set;}
}