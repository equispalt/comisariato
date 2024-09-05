namespace SistemaILP.comisariato.Models
{
    public class MovInventarios
    {
        public long MovInventarioId { get; set; }
        public int ComisariatoId { get; set; }
        public DateTime Fecha { get; set; }
        public long ReferenciaId { get; set; }
        public string Comentario { get; set; }
        public int EstadoId { get; set; }
        public int UsuarioId { get; set; }
        public int ProgramaId { get; set; }
        public DateTime FechaMod { get; set; }
    }

}
