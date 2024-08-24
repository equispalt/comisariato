namespace comisariato.Models
{
    public class FacCompraDet
    {
        public long FacCompraDetId { get; set; }
        public long FacCompraId { get; set; }
        public long ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnidad { get; set; }
        public decimal CostoUnidad { get; set; }
        public decimal IVA_Unidad { get; set; }
        public decimal TotalLinea { get; set; }
        public int EstadoId { get; set; }
        public int UsuarioId { get; set; }
        public int ProgramaId { get; set; }
        public int MonedaId { get; set; }
        public DateTime FechaMod { get; set; }
    }
}
