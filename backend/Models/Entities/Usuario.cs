using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace backend.Models.Entities
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [JsonIgnore] // Para no devolver el password en las consultas API
        public string Password { get; set; } = string.Empty;

        public Rol Rol { get; set; }

        // Relaciones

        [JsonIgnore]
        public ICollection<Curso>? CursosImpartidos { get; set; }

        [JsonIgnore]
        public ICollection<Inscripcion>? Inscripciones { get; set; }
    }
}
