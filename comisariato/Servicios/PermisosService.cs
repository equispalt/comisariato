using Dapper;
using comisariato.Models;
using Microsoft.Data.SqlClient;


namespace comisariato.Servicios
{
    public class PermisosService
    {
        private readonly string connectionString;
        public PermisosService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("ConnectionComisariato");
        }
        public async Task<bool> TienePermiso(string currentUser, string currentFormName)
        {
            int roleId = ObtenerFormIdPorFormNombre(currentUser);
            int formId = ObtenerRoleIdPorUsuarioNombre(currentFormName);

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

        public int ObtenerFormIdPorFormNombre(string formName)
        {
            // Implementación para obtener FormId basado en el nombre del formulario
            return 1; // Valor de ejemplo
        }
        public int ObtenerRoleIdPorUsuarioNombre(string username)
        {
            // Implementación para obtener RoleId basado en el username
            return 1; // Valor de ejemplo
        }



    }

}
