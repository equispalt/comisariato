using comisariato.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace comisariato.Servicios
{
    public interface IAuthService
    {
        Task<int> Login(Usuarios usuarios);
        Task SetSesion(Usuarios usuarios);
    }
    public class AuthService : IAuthService
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;   

        public AuthService(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato");
            _httpContextAccessor = contextAccessor;
        }

        [HttpPost]
        public async Task<int> Login(Usuarios usuario)
        {

            using var connection = new SqlConnection(_connectionString);
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


        public async Task SetSesion(Usuarios usuario)
        {
            List<Claim> sesion = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario.UsuarioName),  // agregar una claim con el nombre del usuario
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()) // agregar una claim con el id del usuario
            };

            ClaimsIdentity identidad = new(sesion, CookieAuthenticationDefaults.AuthenticationScheme); // crear identidad basada en los claims anteiores
            AuthenticationProperties autenticacionPropiedad = new(); //configurar persistencia

            autenticacionPropiedad.AllowRefresh = true; // permitir que la sesion se pueda refrescar
            autenticacionPropiedad.IsPersistent = true; // hacer que la sesion sea persistente
            autenticacionPropiedad.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1); // configurar fecha y hora de expiracion

            // iniciar sesion autenticada en el contexto HTTP usando cookies
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidad), autenticacionPropiedad);
        }
    }

}
