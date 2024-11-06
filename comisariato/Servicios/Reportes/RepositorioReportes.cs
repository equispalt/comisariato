using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.Reportes
{
    public interface IRepositorioReportes 
    {
        Task<List<FacVentas>> ResumenFacturas(DateTime inicio, DateTime fin);
        Task<List<FacVentas>> DetalleVentasPorProducto(DateTime inicio, DateTime fin);
    }


    public class RepositorioReportes : IRepositorioReportes
    {
        private readonly string _connectionString;

        public RepositorioReportes(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato") ?? "";
        }

        public async Task<List<FacVentas>> ResumenFacturas(DateTime inicio, DateTime fin)
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<FacVentas> facturas = await connection.QueryAsync<FacVentas>(@"
                        EXEC resumenFacturasPorFecha  @FechaInicio, @FechaFin",
                        new { 
                            FechaInicio = inicio,
                            FechaFin = fin
                        });
            return facturas.ToList();
        }

        public async Task<List<FacVentas>> DetalleVentasPorProducto(DateTime inicio, DateTime fin)
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<FacVentas> facturas = await connection.QueryAsync<FacVentas>(@"
                        EXEC detalleVentasPorProducto  @FechaInicio, @FechaFin",
                        new
                        {
                            FechaInicio = inicio,
                            FechaFin = fin
                        });
            return facturas.ToList();
        }

    }
}
