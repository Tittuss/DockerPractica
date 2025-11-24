using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using backend.Data;
using backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto request)
        {
            // Buscar usuario por email
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>
                u.Email == request.Email
            );

            if (usuario == null || usuario.Password != request.Password)
            {
                return null;
            }

            var token = GenerarToken(usuario);

            return new AuthResponseDto
            {
                Token = token,
                Nombre = usuario.Nombre,
                Rol = usuario.Rol.ToString(),
            };
        }

        private string GenerarToken(Models.Entities.Usuario usuario)
        {
            var key = _configuration.GetValue<string>("Jwt:Key");
            var keyBytes = Encoding.ASCII.GetBytes(key!);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol.ToString()), // Importante para autorizar endpoints después
            };

            var credenciales = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2), // El token dura 2 horas
                SigningCredentials = credenciales,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(tokenConfig);
        }
    }
}
