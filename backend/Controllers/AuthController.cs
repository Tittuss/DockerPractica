// endpoint oficial que usará el Frontend en Svelte

using backend.Models.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var resultado = await _authService.LoginAsync(loginDto);

            if (resultado == null)
            {
                return Unauthorized(new { message = "Credenciales incorrectas" });
            }

            return Ok(resultado);
        }
    }
}
