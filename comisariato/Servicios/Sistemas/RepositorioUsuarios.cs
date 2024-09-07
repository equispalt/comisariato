using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;
using System.ComponentModel;

namespace SistemaILP.comisariato.Servicios.Sistemas
{
    public interface IRepositorioUsuario 
    {
        Task<List<Usuarios>> ObtieneTodoUsuarios();
    }
    public class RepositorioUsuarios : IRepositorioUsuario
    {
        private readonly string _conectionString;

        public RepositorioUsuarios(IConfiguration configuration)
        {
            _conectionString = configuration.GetConnectionString("ConecctionComisariato");
        }

        public async Task<List<Usuarios>> ObtieneTodoUsuarios() 
        {
            using var connection = new SqlConnection(_conectionString);
            IEnumerable<Usuarios> user = await connection.QueryAsync<Usuarios>(@"
                        EXEC obtieneTodoUsuario
            ");
            return user.ToList();
            
        }

    }
}
