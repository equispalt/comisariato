namespace SistemaILP.comisariato.Models
{
    public class TendenciaVentas
    {
        public string CodigoSILP { get; set; }
        public string NombreProducto { get; set; }
        public int CantidadVendida { get; set; }
        public decimal IngresosGenerados { get; set; }
        public decimal PorcentajeVentas {  get; set; }
        public string Clasificacion {  get; set; }  

    }
}
