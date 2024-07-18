using Microsoft.AspNetCore.Mvc;
using PainelIntegraTelefoniaIP.Entity;
using PainelIntegraTelefoniaIP.Models;
using PainelIntegraTelefoniaIP.Services.Interfaces;

namespace PainelIntegraTelefoniaIP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartamentoController : MeuControllerBase<DepartamentoDtoModel, Departamento>
{
    private readonly IService<DepartamentoDtoModel, Departamento> _service;
    public DepartamentoController(IService<DepartamentoDtoModel, Departamento> service) : base(service)
    {
    }
}