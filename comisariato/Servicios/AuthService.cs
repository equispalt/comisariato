using comisariato.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;

namespace comisariato.Servicios
{
    public interface IAuthService
    {
        Task<int> Login(Usuarios usuarios);
    }
    public class AuthService : IAuthService
    {
        private readonly string connectionString;

        public AuthService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("ConnectionComisariato");
        }

        [HttpPost]
        public async Task<int> Login(Usuarios usuario)
        {

            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var usuarioId = await connection.ExecuteScalarAsync<int>(@"
            EXEC ObtenerUsuarioPorCredencial @UsuarioNombre, @UsuarioPassword
            ", new
            {
                UsuarioNombre = usuario.UsuarioName,
                UsuarioPassword = usuario.UsuarioPassword
            });

            usuario.UsuarioId = usuarioId;

            return usuario.UsuarioId;
        }
    }

}
