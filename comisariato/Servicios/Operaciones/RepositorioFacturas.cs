using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.Operaciones
{
    public interface IRepositorioFacturas
    {
        Task<List<FacVentas>> ObtieneTodoFacturas();
        Task<FacturaDTO> ObtieneEncFactura(int facturaId);
        Task<List<FacturaDTO>> ObtieneDetFactura(int facturaId);
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

        public async Task<FacturaDTO> ObtieneEncFactura(int facturaId)
        {

            using var connection = new SqlConnection(_connectionString);
            IEnumerable<FacturaDTO> encFactura = await connection.QueryAsync<FacturaDTO>(@"
                        EXEC obtieneEncFactura @facventaid
                     ", new
            {
                facventaid = facturaId
            });

            return encFactura.FirstOrDefault();
        }
        public async Task<List<FacturaDTO>> ObtieneDetFactura(int facturaId)
        {

            using var connection = new SqlConnection(_connectionString);
            IEnumerable<FacturaDTO> detfacturas = await connection.QueryAsync<FacturaDTO>(@"
                        EXEC obtieneDetFactura @facventaid
                     ", new
            {
                facventaid = facturaId
            });

            return detfacturas.ToList();
        }


    }
}
