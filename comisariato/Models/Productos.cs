namespace SistemaILP.comisariato.Models
{
    public class Productos
    {
        public int ProductoId { get; set; }
        public string CodigoSILP { get; set; }
        public string Nombre { get; set; }
        public string CodigoBarra { get; set; }
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Categoria { get; set; }
        public bool OrigenExterno { get; set; }
        public decimal Precio { get; set; }
        public decimal Costo { get; set; }
        public int EstadoId { get; set; }
        public int ProgramaId { get; set; }
        public int MonedaId { get; set; }
        public DateTime FechaMod { get; set; }

        // relacionado a modelo estados
        public string NombreEstado { get; set; }
    }
}
