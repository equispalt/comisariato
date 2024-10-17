using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.Sistemas
{
    public interface IRepositorioComisariatos
    {
        Task<List<Comisariato>> ObtieneTodoComisariatos();
    }

    public class RepositorioComisariatos : IRepositorioComisariatos
    {
        private readonly string _connectionString;

        public RepositorioComisariatos(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato") ?? "";
        }

        public async Task<List<Comisariato>> ObtieneTodoComisariatos()
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<Comisariato> comisariato = await connection.QueryAsync<Comisariato>(@"
                        EXEC obtieneTodoComisariato
            ");
            return comisariato.ToList();
        }
    }
}
