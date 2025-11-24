namespace backend.Models.DTOs
{
    public class CursoResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string NombreDocente { get; set; } = string.Empty;

        public float? Promedio { get; set; }
    }
}
