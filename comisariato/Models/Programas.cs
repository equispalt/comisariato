﻿namespace SistemaILP.comisariato.Models
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
        public int Estado { get; set; }
        public DateTime FechaMod { get; set; }

        // Navigation property


    }
}