namespace comisariato.Models
{
    public class Empleado
    {
        public long EmpleadoId { get; set; }
        public string Codigo { get; set; }
        public string FolioCorporativo { get; set; }
        public string Nombre { get; set; }
        public string NIT { get; set; }
        public string DPI { get; set; }
        public int EstadoId { get; set; }
        public int ProgramaId { get; set; }
        public DateTime FechaMod { get; set; }
    }
}
