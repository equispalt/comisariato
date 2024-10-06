using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.MercadeoVentas
{
    public interface IRepositorioProducto
    {
        Task<List<Productos>> ObtieneTodoProductos();
    }
    public class RepositorioProductos : IRepositorioProducto
    {
        private readonly string _connectionString;


        public RepositorioProductos(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato") ?? "";
        }

        public async Task<List<Productos>> ObtieneTodoProductos()
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<Productos> pro = await connection.QueryAsync<Productos>(@"
                    EXEC obtieneTodoProducto
            ");

            return pro.ToList();
        }
    }
}
