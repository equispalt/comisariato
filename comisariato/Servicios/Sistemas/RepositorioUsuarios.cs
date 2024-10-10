using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.Sistemas
{
    public interface IRepositorioUsuario
    {
        Task<List<Usuarios>> ObtieneTodoUsuarios();
        Task<Usuarios> ObtienePorUsuarioId(int id);
        Task<bool> PaValidarUsuario(string usuario);
        Task<bool> PaCrearUsuario(Usuarios usuario);
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

        public async Task<bool> PaValidarUsuario(string usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            var existe = await connection.ExecuteScalarAsync<bool>(@"
            EXEC paValidarUsuario @usuario", 
            new { usuario });
            return existe;
        }


        public async Task<bool> PaCrearUsuario(Usuarios usuario) 
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(@"
                EXEC paCrearUsuario @usuario, @password, @empleadoid, @rolid",
                new { 
                    usuario = usuario.Usuario,
                    password = usuario.Password,
                    empleadoid = usuario.EmpleadoId,
                    rolid     = usuario.RolId
                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> PaEditarUsuario(Usuarios usuario)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(@"
                       EXEC paEditarUsuario @usuarioid, @estadoid ",
                       new
                       {
                           usuarioid = usuario.UsuarioId,
                           estadoid =usuario.EstadoId,
                       });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> PaEditarPassword(Usuarios usuario)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(@"
                       EXEC paEditarPassword @usuarioid, @password",
                       new
                       {
                           usuarioid = usuario.UsuarioId,
                           password = usuario.Password,
                       });
                return true;
            }
            catch (Exception ex)
            {
                return false;
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
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
