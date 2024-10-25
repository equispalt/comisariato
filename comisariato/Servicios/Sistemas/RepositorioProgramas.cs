using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.Sistemas
{
    public interface IRepositorioProgramas
    {
        Task<List<Programas>> ObtieneTodoProgramas();
    }
    public class RepositorioProgramas : IRepositorioProgramas
    {
        private readonly string _connectionString;


        public RepositorioProgramas(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato") ?? "";
        }

        public async Task<List<Programas>> ObtieneTodoProgramas()
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<Programas> programas = await connection.QueryAsync<Programas>(@"
                        EXEC obtieneTodoPrograma
            ");
            return programas.ToList();
        }

    }
}
