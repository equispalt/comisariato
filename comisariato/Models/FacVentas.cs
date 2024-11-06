namespace SistemaILP.comisariato.Models
{
    public class FacVentas
    {
        public long FacVentaId { get; set; }
        public int ComisariatoId { get; set; }
        public string Serie { get; set; }
        public string Consecutivo { get; set; }
        public string UUID { get; set; }
        public long? EmpleadoId { get; set; }
        public decimal Descuento { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
        public int EstadoId { get; set; }
        public int ProgramaId { get; set; }
        public DateTime FechaMod { get; set; }
        public int UsuarioId { get; set; }
        public int TipoPagoId { get; set; }
        public int Empresaid { get; set; }
        public int MonedaId { get; set; }

        // otras referencias

        public List<FacVentasDet> lstFacVentasDet { get; set; }

        public string NombreEmpleado { get; set; }
        public string? NIT { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreProducto { get; set; }
        public string NombreComisariato { get; set; }
        public string NombreEstado { get; set; }
        public string NombreTipoPago { get; set; }
        public int CantidadVendida { get; set; }
        public decimal PrecioUnidad { get; set; }


    }
}