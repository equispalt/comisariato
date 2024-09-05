namespace SistemaILP.comisariato.Models
{
    public class Existencia
    {
        public long ExistenciaId { get; set; }
        public int ComisariatoId { get; set; }
        public long ProductoId { get; set; }
        public int Disponible { get; set; }
        public int Transito { get; set; }
        public int MalEstado { get; set; }
        public decimal Costo { get; set; }
        public int EstadoId { get; set; }
        public int ProgramaId { get; set; }
        public int MonedaId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaMod { get; set; }
    }

}
