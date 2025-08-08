using Microsoft.AspNetCore.Mvc;

namespace PainelIntegraTelefoniaIP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController
    {
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LoginRequest request)
        {
            if (request.Username == "admin" && request.Password == "password")
            {
                return Ok(new { Token = "fake-jwt-token" });
            }

            return Unauthorized(new { Message = "Invalid username or password" });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}