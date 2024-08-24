namespace comisariato.Models
{
    public class Configuracion
    {
        public int ConfiguracionId { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public int EstadoId { get; set; }
        public int ProgramaId { get; set; }
        public DateTime FechaMod { get; set; }
    }
}
