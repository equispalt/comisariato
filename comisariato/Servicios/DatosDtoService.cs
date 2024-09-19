using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios
{
    public interface IDatosDtoService
    {
        Task<List<Estados>> ObtieneTodoEstados();
        Task<List<Roles>> ObtieneTodoRoles();
    }
    public class DatosDtoService : IDatosDtoService
    {
        private readonly string _connectionString;

        public DatosDtoService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato")?? "";
        }

        public async Task<List<Estados>> ObtieneTodoEstados()
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<Estados> estados = await connection.QueryAsync<Estados>(@" 
                EXEC obtieneEstados
                ");
            return estados.ToList();
        }

        public async Task<List<Roles>> ObtieneTodoRoles()
        { 
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<Roles> roles = await connection.QueryAsync<Roles>(@"
                EXEC obtieneRoles
                ");
            return roles.ToList();
        }



    }
}
