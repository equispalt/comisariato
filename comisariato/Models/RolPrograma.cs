namespace SistemaILP.comisariato.Models
{
    public class RolPrograma
    {
        public int RolProgramaId { get; set; }
        public int RolId { get; set; }
        public int ProgramaId { get; set; }
        public int EstadoId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaMod { get; set; }
    }
}
