using Microsoft.EntityFrameworkCore;

namespace SistemaILP.comisariato.Data
{
    public class Paginacion<T> : List<T>
    {
        public int PaginaInicio { get; private set; }
        public int PaginasTotales { get; private set; }
        public Paginacion(List<T> items, int contador, int paginaInicio, int cantidadregistros) 
        {
            PaginaInicio = paginaInicio;
            PaginasTotales = (int)Math.Ceiling(contador / (double)cantidadregistros);

            this.AddRange(items);
        }
        public bool PaginasAnteriores => PaginaInicio > 1;
        public bool PaginasPosteriores => PaginaInicio < PaginasTotales;
        public static Paginacion<T> CrearPaginacion(List<T> fuente, int paginaInicio, int cantidadregistros)
        {
            var contador = fuente.Count();
            var item = fuente.Skip((paginaInicio - 1) * cantidadregistros).Take(cantidadregistros).ToList();
            return new Paginacion<T>(item, contador, paginaInicio, cantidadregistros);
        }
    }
}
