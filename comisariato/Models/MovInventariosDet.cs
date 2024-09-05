namespace SistemaILP.comisariato.Models
{
    public class MovInventariosDet
    {
        public long MovInventarioDetId { get; set; }
        public long MovInventarioId { get; set; }
        public long ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal CostoUnidad { get; set; }
        public int EstadoId { get; set; }
        public int UsuarioId { get; set; }
        public int ProgramaId { get; set; }
        public int MonedaId { get; set; }
        public DateTime FechaMod { get; set; }
    }

}
