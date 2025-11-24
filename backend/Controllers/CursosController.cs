using System.Security.Claims;
using backend.Data;
using backend.Models.DTOs;
using backend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Usuarios con Token valido
    public class CursosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CursosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("mis-cursos")]
        public async Task<IActionResult> GetMisCursos()
        {
            // Extrae el ID y el Rol del Token
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRoleString = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            List<CursoResponseDto> cursosResultado = new List<CursoResponseDto>();

            // Logica segun el rol
            // Docente
            if (userRoleString == Rol.Docente.ToString())
            {
                cursosResultado = await _context
                    .Cursos.Where(c => c.DocenteId == userId)
                    .Select(c => new CursoResponseDto
                    {
                        Id = c.Id,
                        Nombre = c.Nombre,
                        Descripcion = c.Descripcion,
                        NombreDocente = "Yo (Docente)",
                    })
                    .ToListAsync();
            }
            // Estudiante
            else if (userRoleString == Rol.Estudiante.ToString())
            {
                cursosResultado = await _context
                    .Inscripciones.Include(i => i.Curso)
                        .ThenInclude(c => c.Docente)
                    .Where(i => i.EstudianteId == userId)
                    .Select(i => new CursoResponseDto
                    {
                        Id = i.Curso!.Id,
                        Nombre = i.Curso.Nombre,
                        Descripcion = i.Curso.Descripcion,
                        NombreDocente =
                            i.Curso.Docente != null ? i.Curso.Docente.Nombre : "Sin asignar",
                        Promedio = (i.Nota1 + i.Nota2 + i.Nota3) / 3,
                    })
                    .ToListAsync();
            }

            return Ok(cursosResultado);
        }

        [HttpGet("{id}/estudiantes")]
        public async Task<IActionResult> GetEstudiantesPorCurso(int id)
        {
            var userId = int.Parse(
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
            );
            var rol = User.FindFirst(System.Security.Claims.ClaimTypes.Role)!.Value;

            var curso = await _context
                .Cursos.Include(c => c.Inscripciones!)
                    .ThenInclude(i => i.Estudiante)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso == null)
                return NotFound("Curso no encontrado.");

            // Si es docente, verificar que sea SU curso
            if (rol == "Docente" && curso.DocenteId != userId)
                return Unauthorized("No tienes permiso para ver este curso.");

            var listado = curso
                .Inscripciones!.Select(i => new InscripcionDto
                {
                    Id = i.Id,
                    EstudianteId = i.EstudianteId,
                    NombreEstudiante = i.Estudiante!.Nombre,
                    Nota1 = i.Nota1,
                    Nota2 = i.Nota2,
                    Nota3 = i.Nota3,
                    NotaFinal = (i.Nota1 + i.Nota2 + i.Nota3) / 3, // promedio
                })
                .ToList();

            return Ok(listado);
        }
    }
}
