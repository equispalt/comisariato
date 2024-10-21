using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.Operaciones
{
    public interface IRepositorioFacturas
    {
        Task<List<FacVentas>> ObtieneTodoFacturas();
    }
    public class RepositorioFacturas : IRepositorioFacturas
    {
        private readonly string _connectionString;

        public RepositorioFacturas(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato") ?? "";
        }

        public async Task<List<FacVentas>> ObtieneTodoFacturas()
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<FacVentas> facturas = await connection.QueryAsync<FacVentas>(@"
                        EXEC obtieneTodoFactura
            ");
            return facturas.ToList();
        }

    }
}
