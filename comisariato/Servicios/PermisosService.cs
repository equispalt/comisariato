using Dapper;
using comisariato.Models;
using Microsoft.Data.SqlClient;
using System.Runtime.InteropServices.ObjectiveC;


namespace comisariato.Servicios
{
    public class PermisosService
    {
        private readonly string connectionString;
        public PermisosService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("ConnectionComisariato")??"";
        }
        public async Task<bool> TienePermiso(string currentUser, string currentFormName)
        {
            int roleId = await ObtenerFormIdPorFormNombre(currentUser);
            int formId = await ObtenerRoleIdPorUsuarioNombre(currentFormName);

            if (roleId != -1 && formId != -1)
            {
                using var connection = new SqlConnection(connectionString);
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
            using var conection = new SqlConnection(connectionString);
            try
            {
                int result = await conection.ExecuteScalarAsync<int>(@"
                EXEC SP_ObtenerFormPorNombre @FormNombre
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
                using var conection = new SqlConnection(connectionString);

                int result = await conection.ExecuteScalarAsync<int>(@"
                EXEC SP_ObtenerFormIdPorFormNombre @FormNombre
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
