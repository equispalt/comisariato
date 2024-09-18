using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;
using System.ComponentModel;
using System.Data;

namespace SistemaILP.comisariato.Servicios.Sistemas
{
    public interface IRepositorioUsuario 
    {
        Task<List<Usuarios>> ObtieneTodoUsuarios();
        Task<Usuarios> ObtienePorUsuarioId(int id);
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
                       EXEC SP_EDITAR_USUARIO @idusuario, @nombre_usuario, @contrasennia, @correo, @Id_role
                        ", new
                {
                    UsuarioId = usuario.UsuarioId,
                    Usuario  = usuario.Usuario,
                    Password = usuario.Password,
                    RolId = usuario.RolId,
                    EstadoId = usuario.EstadoId
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
