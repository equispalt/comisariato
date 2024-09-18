using Dapper;
using Microsoft.Data.SqlClient;
using System.Security.Claims;


namespace SistemaILP.comisariato.Servicios
{
    public interface IPermisosService
    {
        Task<bool> esPermitido(string user, string program);
        Task<bool> ValidaPermisoPrograma();
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

        public async Task<bool> ValidaPermisoPrograma()
        {
            string currentUser = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (!string.IsNullOrEmpty(currentUser))
            {
                //string currentProgramName = Path.GetFileNameWithoutExtension(_contextAccessor.HttpContext.Request.Path);


                string currentProgramName = _contextAccessor.HttpContext.Request.Path;

                bool esAutorizado = await esPermitido(currentUser, currentProgramName);

                return esAutorizado;
            }
            return false;
        }


        public async Task<bool> esPermitido(string currentUser, string currentProgramName)
        {
            int programId = await ObtieneProgramaIdPorNombrePrograma(currentProgramName);
            int roleId = await ObtieneRolIdPorUsuario(currentUser);

            if (roleId != -1 && programId != -1)
            {
                using var connection = new SqlConnection(_connectionString);
                try
                {
                    int TienePermiso = await connection.ExecuteScalarAsync<int>(@"
                    EXEC obtieneRecuentoPermiso @rolId, @programaId
                ", new
                    {
                        rolId = roleId,
                        programaId = programId
                    });
                    return TienePermiso > 0;
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    throw new Exception($"Error al verificar permiso para el usuario {currentUser} en el programa {currentProgramName}: {ex.Message}", ex);
                }
            }
            return false;
        }

        public async Task<int> ObtieneProgramaIdPorNombrePrograma(string path)
        {
            using var conection = new SqlConnection(_connectionString);
            try
            {
                int result = await conection.ExecuteScalarAsync<int>(@"
                EXEC obtieneProgramaIdPorNombrePrograma @path
            ", new
                {
                    path = path
                });
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el ProgramaId para el usuario {path}: {ex.Message}", ex);
            }
        }
        public async Task<int> ObtieneRolIdPorUsuario(string user)
        {
            try
            {
                using var conection = new SqlConnection(_connectionString);

                int result = await conection.ExecuteScalarAsync<int>(@"
                EXEC obtieneRolIdPorUsuario @usuario
            ", new
                {
                    usuario = user
                });
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el RolId para el usuario {user}: {ex.Message}", ex);
            }
        }
    }
}
