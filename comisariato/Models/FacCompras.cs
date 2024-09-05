namespace SistemaILP.comisariato.Models
{
    public class FacCompras
    {
        public long FacCompraId { get; set; }
        public int ComisariatoId { get; set; }
        public string Serie { get; set; }
        public string Consecutivo { get; set; }
        public DateTime Fecha { get; set; }
        public string NIT_DPI { get; set; }
        public string RazonSocial { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
        public int EstadoId { get; set; }
        public int UsuarioId { get; set; }
        public int ProgramaId { get; set; }
        public int MonedaId { get; set; }
        public DateTime FechaMod { get; set; }
    }

}
