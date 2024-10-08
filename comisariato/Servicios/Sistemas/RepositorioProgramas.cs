using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.Sistemas
{
    public class RepositorioProgramas
    {
        private readonly string _connectionString;


        public RepositorioProgramas(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato")??"";
        }

        public async Task<List<Programas>> ObtieneTodoRoles()
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<Programas> programas = await connection.QueryAsync<Programas>(@"
                        EXEC obtieneTodoRoles
            ");
            return programas.ToList();
        }

    }
}
