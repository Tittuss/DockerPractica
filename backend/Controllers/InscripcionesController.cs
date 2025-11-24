using backend.Data;
using backend.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Docente")]
    public class InscripcionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InscripcionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPut("{id}/notas")]
        public async Task<IActionResult> ActualizarNotas(
            int id,
            [FromBody] ActualizarNotasDto notas
        )
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);

            if (inscripcion == null)
                return NotFound("Inscripción no encontrada");

            // Actualizamos valores
            inscripcion.Nota1 = notas.Nota1;
            inscripcion.Nota2 = notas.Nota2;
            inscripcion.Nota3 = notas.Nota3;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Notas actualizadas correctamente" });
        }
    }
}
