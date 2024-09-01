using Dapper;
using comisariato.Models;
using Microsoft.Data.SqlClient;
using System.Runtime.InteropServices.ObjectiveC;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace comisariato.Servicios
{
    public interface IPermisosService 
    {
        Task<bool> TienePermiso(string user, string form);
        Task<bool> ValidaPermisoForm();
    }
    public class PermisosService : IPermisosService
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _contextAccessor;
        public PermisosService(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato") ?? "";
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> ValidaPermisoForm() 
        {
            string currentUser = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (!string.IsNullOrEmpty(currentUser)) 
            {
                string currentFormName = System.IO.Path.GetFileNameWithoutExtension(_contextAccessor.HttpContext.Request.Path);

                bool tienePermiso = await TienePermiso(currentUser, currentFormName);

                return tienePermiso;
            }
            return false;
        }


        public async Task<bool> TienePermiso(string currentUser, string currentFormName)
        {
            int formId = await ObtenerFormIdPorFormNombre(currentFormName);
            int roleId = await ObtenerRoleIdPorUsuarioNombre(currentUser);

            if (roleId != -1 && formId != -1)
            {
                using var connection = new SqlConnection(_connectionString);
                try
                {
                    int TienePermiso = await connection.ExecuteScalarAsync<int>(@"
                    EXEC ObtenerRecuentoPermisos @RoleId, @FormId
                ", new
                    {
                        RoleId = roleId,
                        FormId = formId
                    });
                    return TienePermiso > 0;
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    return false;
                }
            }
            return false;
        }

        public async Task<int> ObtenerFormIdPorFormNombre(string formName)
        {
            using var conection = new SqlConnection(_connectionString);
            try
            {
                int result = await conection.ExecuteScalarAsync<int>(@"
                EXEC ObtenerFormPorNombre @FormNombre
            ", new
                {
                    FormName = formName
                });
                return result;
            }
            catch (Exception ex)
            {
                return 0; 
            }
        }
        public async Task<int> ObtenerRoleIdPorUsuarioNombre(string username)
        {
            try
            {
                using var conection = new SqlConnection(_connectionString);

                int result = await conection.ExecuteScalarAsync<int>(@"
                EXEC ObtenerRoleIdPorUsuarioNombre @UsuarioNombre
            ", new
                {
                    UsuarioNombre = username
                });
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
