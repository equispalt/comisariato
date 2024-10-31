using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.Operaciones
{
    public interface IRepositorioFacturas
    {
        Task<List<FacVentas>> ObtieneTodoFacturas();
        Task<FacturaDTO> ObtieneFactura(int facturaId);
        Task<FacturaDTO> PaObtenerEmpleadoPorNit(string nit);
        Task<DetalleFacturaDTO> PaObtenerProductoPorCodigo(string codigo);
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

        public async Task<FacturaDTO> ObtieneFactura(int facturaId)
        {
            using var connection = new SqlConnection(_connectionString);

            // Obtener el encabezado de la factura
            var facturaEncabezado = await connection.QuerySingleOrDefaultAsync<FacturaDTO>(@"
                EXEC obtieneEncFactura @facventaid
             ", new
            {
                facventaid = facturaId
            });

            // Obtener los detalles de la factura
            var detalles = await connection.QueryAsync<DetalleFacturaDTO>(@"
                EXEC obtieneDetFactura @facventaid
             ", new
            {
                facventaid = facturaId
            });

            // Asignar detalles a la factura
            if (facturaEncabezado != null)
            {
                facturaEncabezado.DetallesFactura = detalles.ToList();
            }

            return facturaEncabezado;
        }

        // PERMITE CONSULTAR, CLIENTES, PRODUCTOS Y EXITENCIAS

        public async Task<FacturaDTO> PaObtenerEmpleadoPorNit(string nit)
        {
            using var connection = new SqlConnection(_connectionString);
            var empleado = await connection.QueryFirstOrDefaultAsync<FacturaDTO>(@"
            EXEC obtieneEmpleadoPorNitDTO @nit",
                new { nit });

            return empleado; // Retorna null si no se encuentra el empleado
        }

        public async Task<DetalleFacturaDTO> PaObtenerProductoPorCodigo(string codigo)
        {
            using var connection = new SqlConnection(_connectionString);
            var producto = await connection.QueryFirstOrDefaultAsync<DetalleFacturaDTO>(@"
            EXEC obtieneProductoPorCodigoDTO @codigo",
                new { codigo });

            return producto; // Retorna null si no se encuentra el producto
        }

    }
}
