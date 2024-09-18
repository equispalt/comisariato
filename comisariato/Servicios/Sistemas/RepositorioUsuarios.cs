using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.Sistemas
{
    public interface IRepositorioUsuario
    {
        Task<List<Usuarios>> ObtieneTodoUsuarios();
        Task<Usuarios> ObtienePorUsuarioId(int id);
        Task<bool> PaEditarUsuario(Usuarios usuario);
        Task<bool> PaEditarPassword(Usuarios usuario);
        Task<bool> PaEliminarUsuario(int id);
    }
    public class RepositorioUsuarios : IRepositorioUsuario
    {
        private readonly string _connectionString;

        public RepositorioUsuarios(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato") ?? "";
        }

        public async Task<List<Usuarios>> ObtieneTodoUsuarios()
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<Usuarios> user = await connection.QueryAsync<Usuarios>(@"
                        EXEC obtieneTodoUsuario
            ");
            return user.ToList();
        }
        public async Task<Usuarios> ObtienePorUsuarioId(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<Usuarios> user = await connection.QueryAsync<Usuarios>(@"
                        EXEC obtieneUsuarioPorId @usuarioid 
                        ", new
            {
                usuarioid = id
            });
            return user.FirstOrDefault();
        }

        public async Task<bool> PaEditarUsuario(Usuarios usuario)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(@"
                       EXEC paEditarUsuario @usuarioid, @usuario",
                       new
                       {
                           usuarioid = usuario.UsuarioId,
                           usuario = usuario.Usuario,
                       });
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> PaEditarPassword(Usuarios usuario)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(@"
                       EXEC paEditarUsuario @usuarioid, @password",
                       new
                       {
                           usuarioid = usuario.UsuarioId,
                           password = usuario.Password,
                       });
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> PaEliminarUsuario(int userId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(@"
                    EXEC paEliminarUsuario @usuarioid",
                    new
                    {
                        usuarioid = userId
                    });
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
