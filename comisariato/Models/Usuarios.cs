namespace SistemaILP.comisariato.Models
{
    public class Usuarios
    {
        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public int EmpleadoId { get; set; }
        public int RolId { get; set; }
        public int EstadoId { get; set; }

    }
}
