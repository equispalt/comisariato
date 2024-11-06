namespace SistemaILP.comisariato.Models
{
    public class FacturaDTO
    {
        // Propiedades de la factura (FacVentas)
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

        // Otras referencias
        public string NombreEmpleado { get; set; }
        public string NIT { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreComisariato { get; set; }
        public string NombreEstado { get; set; }
        public string NombreTipoPago { get; set; }

        // Detalles de la factura (FacVentasDet) agrupados en una lista
        public List<DetalleFacturaDTO> DetallesFactura { get; set; }

        // Constructor para inicializar la lista
        public FacturaDTO()
        {
            DetallesFactura = new List<DetalleFacturaDTO>();
        }
    }

    public class DetalleFacturaDTO
    {
        public long FacVentasDetId { get; set; }
        public long FacVentasId { get; set; }
        public long ProductoId { get; set; }
        public string CodigoSILP { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public int? Cantidad { get; set; }
        public decimal DescuentoUnidad { get; set; }
        public decimal IVA_Unidad { get; set; }
        public decimal PrecioUnidad { get; set; }
        public decimal TotalLinea { get; set; }
        public int Disponible { get; set; }

        public string Mensaje { get; set; }
    }


}
