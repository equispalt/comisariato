namespace SistemaILP.comisariato.Models
{
    public class Roles
    {
        public int RolId { get; set; }
        public string Nombre { get; set; }
        public int EstadoId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaMod { get; set; }
        public string Descripcion { get; set; }
        public int ProgramaId { get; set; }

        // relacionado a modelo estados
        public string NombreEstado { get; set; }
    }
}
