using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Tablas
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuracion adicional
            // Un Docente puede tener muchos cursos
            modelBuilder
                .Entity<Curso>()
                .HasOne(c => c.Docente)
                .WithMany(u => u.CursosImpartidos)
                .HasForeignKey(c => c.DocenteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Un Estudiante puede tener muchas inscripciones
            modelBuilder
                .Entity<Inscripcion>()
                .HasOne(i => i.Estudiante)
                .WithMany(u => u.Inscripciones)
                .HasForeignKey(i => i.EstudianteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
