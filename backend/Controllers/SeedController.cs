using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SeedController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ExecuteSeed()
        {
            if (await _context.Usuarios.AnyAsync())
            {
                return Ok(new { message = "La base de datos ya tiene datos." });
            }

            var docente = new Usuario
            {
                Nombre = "Profesor Juan",
                Email = "docente@iglesia.com",
                Password = "123",
                Rol = Rol.Docente,
            };

            var estudiante = new Usuario
            {
                Nombre = "Alumno Pedro",
                Email = "alumno@iglesia.com",
                Password = "123",
                Rol = Rol.Estudiante,
            };

            var admin = new Usuario
            {
                Nombre = "Administrador",
                Email = "admin@iglesia.com",
                Password = "123",
                Rol = Rol.Admin,
            };

            _context.Usuarios.AddRange(docente, estudiante, admin);
            await _context.SaveChangesAsync();

            var curso1 = new Curso
            {
                Nombre = "Teología Sistemática I",
                Descripcion = "Introducción a las doctrinas fundamentales.",
                DocenteId = docente.Id,
            };

            var curso2 = new Curso
            {
                Nombre = "Historia de la Iglesia",
                Descripcion = "Desde los apóstoles hasta la reforma.",
                DocenteId = docente.Id,
            };

            _context.Cursos.AddRange(curso1, curso2);
            await _context.SaveChangesAsync();

            var inscripcion = new Inscripcion
            {
                EstudianteId = estudiante.Id,
                CursoId = curso1.Id,
                FechaInscripcion = DateTime.UtcNow,
            };

            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();

            return Ok(
                new { message = "Datos de prueba creados exitosamente (Docente, Alumno, Cursos)." }
            );
        }
    }
}
