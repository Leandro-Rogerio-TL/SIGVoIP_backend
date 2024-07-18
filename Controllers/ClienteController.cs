using Microsoft.AspNetCore.Mvc;
using PainelIntegraTelefoniaIP.Entity;
using PainelIntegraTelefoniaIP.Models;
using PainelIntegraTelefoniaIP.Services.Interfaces;

namespace PainelIntegraTelefoniaIP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : MeuControllerBase<CreateClienteModel, Cliente>
{
    private readonly IService<CreateClienteModel, Cliente> _service;
    public ClienteController(IService<CreateClienteModel, Cliente> service) : base(service)
    {
    }
}