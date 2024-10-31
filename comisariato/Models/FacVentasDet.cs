namespace SistemaILP.comisariato.Models
{
    public class FacVentasDet
    {
        public long FacVentasDetId { get; set; }
        public long FacVentasId { get; set; }
        public long ProductoId { get; set; }
        public int? Cantidad { get; set; }
        public decimal CostoUnidad { get; set; }
        public decimal DescuentoUnidad { get; set; }
        public decimal IVA_Unidad { get; set; }
        public decimal PrecioUnidad { get; set; }
        public decimal TotalLinea { get; set; }
        public int ProgramaId { get; set; }
        public DateTime FechaMod { get; set; }

        // Otras referencias
        public string CodigoSILP { get; set; }  

    }

}
