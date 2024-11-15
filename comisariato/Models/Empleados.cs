﻿namespace SistemaILP.comisariato.Models
{
    public class Empleados
    {
        public int EmpleadoId { get; set; }
        public string Codigo { get; set; }
        public string FolioCorporativo { get; set; }
        public string Nombre { get; set; }
        public string NIT { get; set; }
        public string DPI { get; set; }
        public int EstadoId { get; set; }
        public int ProgramaId { get; set; }
        public DateTime FechaMod { get; set; }

        // relaciona a estados
        public string NombreEstado { get; set; }
    }
}
