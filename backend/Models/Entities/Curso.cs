using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Entities
{
    public class Curso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        // Relación con el Docente
        public int? DocenteId { get; set; }

        [ForeignKey("DocenteId")]
        public Usuario? Docente { get; set; }

        // Lista de inscritos
        public ICollection<Inscripcion>? Inscripciones { get; set; }
    }
}
