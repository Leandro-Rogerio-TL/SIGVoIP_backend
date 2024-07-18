using Microsoft.AspNetCore.Mvc;
using PainelIntegraTelefoniaIP.Entity;
using PainelIntegraTelefoniaIP.Models;
using PainelIntegraTelefoniaIP.Services.Interfaces;

namespace PainelIntegraTelefoniaIP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FuncionarioController : MeuControllerBase<CreateFuncionarioModel, Funcionario>
{
    private readonly IService<CreateFuncionarioModel, Funcionario> _service;
    public FuncionarioController(IService<CreateFuncionarioModel, Funcionario> service) : base(service)
    {
    }
}