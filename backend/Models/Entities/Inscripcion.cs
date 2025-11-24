using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Entities
{
    public class Inscripcion
    {
        [Key]
        public int Id { get; set; }

        public int EstudianteId { get; set; }

        [ForeignKey("EstudianteId")]
        public Usuario? Estudiante { get; set; }

        public int CursoId { get; set; }

        [ForeignKey("CursoId")]
        public Curso? Curso { get; set; }

        public DateTime FechaInscripcion { get; set; } = DateTime.UtcNow;

        public float Nota1 { get; set; }
        public float Nota2 { get; set; }
        public float Nota3 { get; set; }
    }
}
