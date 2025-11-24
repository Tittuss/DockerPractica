namespace backend.Models.DTOs
{
    public class InscripcionDto
    {
        public int Id { get; set; } // ID de la inscripción
        public int EstudianteId { get; set; }
        public string NombreEstudiante { get; set; } = string.Empty;

        // Las notas
        public float Nota1 { get; set; }
        public float Nota2 { get; set; }
        public float Nota3 { get; set; }
        public float NotaFinal { get; set; } // Calculada
    }
}
