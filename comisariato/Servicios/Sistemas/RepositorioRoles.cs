using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.Sistemas
{
    public interface IRepositorioRoles
    {
        Task<List<Roles>> ObtieneTodoRoles();
    }

    public class RepositorioRoles : IRepositorioRoles
    {
        private readonly string _connectionString;

        public RepositorioRoles(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato") ?? "";
        }

        public async Task<List<Roles>> ObtieneTodoRoles()
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<Roles> rol = await connection.QueryAsync<Roles>(@"
                        EXEC obtieneTodoRoles
            ");
            return rol.ToList();
        }



    }
}
