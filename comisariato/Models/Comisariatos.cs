namespace SistemaILP.comisariato.Models
{
    public class Comisariato
    {
        public int ComisariatoId { get; set; }
        public string Nombre { get; set; }
        public long ResponsableId { get; set; }
        public int EstadoId { get; set; }
        public int ProgramaId { get; set; }
        public DateTime FechaMod { get; set; }
    }
}
