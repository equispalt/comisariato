namespace SistemaILP.comisariato.Models
{
    public class Programas
    {
        public int ProgramaId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int EsMenu { get; set; }
        public string PathURL { get; set; }
        public int TipoProgramaId { get; set; }
        public int? ProgramaPadreId { get; set; }
        public string Icono { get; set; }
        public int EstadoId { get; set; }
        public DateTime FechaMod { get; set; }

        // Relacionado a 

        public string NombreTipoPrograma { get; set; }
        public string NombreEstado { get; set; }
        public string Ruta { get; set; }


    }
}
