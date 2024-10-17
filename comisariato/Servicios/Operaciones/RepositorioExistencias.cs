using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.Operaciones
{
    public interface IRepositorioExistencias
    {
        Task<List<Existencias>> ObtieneTodoExistencia();
    }

    public class RepositorioExistencias : IRepositorioExistencias
    {
        private readonly string _connectionString;

        public RepositorioExistencias(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato") ?? "";
        }

        public async Task<List<Existencias>> ObtieneTodoExistencia()
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<Existencias> existencia = await connection.QueryAsync<Existencias>(@"
                        EXEC obtieneTodoExistencia
            ");
            return existencia.ToList();
        }
    }
}