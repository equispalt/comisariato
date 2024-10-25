using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.Finanzas
{
    public interface IRepositorioEmpleado
    {
        Task<List<Empleados>> ObtieneTodoEmpleados();
        Task<Empleados> ObtienePorEmpleadoId(int id);
        Task<bool> PaValidarCodigoEmpleado(string codigo);
        Task<bool> PaCrearEmpleado(Empleados empleado);
        Task<bool> PaEditarEmpleado(Empleados empleado);
        Task<bool> PaEliminarEmpleado(int id);
    }
    public class RepositorioEmpleados : IRepositorioEmpleado
    {
        private readonly string _connectionString;

        public RepositorioEmpleados(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato") ?? "";
        }

        public async Task<List<Empleados>> ObtieneTodoEmpleados()
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<Empleados> emp = await connection.QueryAsync<Empleados>(@"
                    EXEC obtieneTodoEmpleado
            ");

            return emp.ToList();
        }

        public async Task<Empleados> ObtienePorEmpleadoId(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<Empleados> emp = await connection.QueryAsync<Empleados>(@"
                    EXEC  obtieneEmpleadoPorId @empleadoid",
                new
                {
                    empleadoid = id
                });
            return emp.FirstOrDefault();
        }

        public async Task<bool> PaValidarCodigoEmpleado(string codigo)
        {
            using var connection = new SqlConnection(_connectionString);
            var existe = await connection.ExecuteScalarAsync<bool>(@"
            EXEC paValidarCodigoEmpleado @codigo",
            new { codigo });
            return existe;
        }

        public async Task<bool> PaCrearEmpleado(Empleados empleado)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(@"
                EXEC paCrearEmpleado @codigo, @nombre, @nit, @dpi ",
                new
                {
                    codigo = empleado.Codigo,
                    nombre = empleado.Nombre,
                    nit = empleado.NIT,
                    dpi = empleado.DPI,
                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> PaEditarEmpleado(Empleados empleado)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(@"
                       EXEC paEditarEmpleado @empleadoid, @nombre, @nit, @dpi ",
                       new
                       {
                           empleadoid = empleado.EmpleadoId,
                           nombre = empleado.Nombre,
                           nit = empleado.NIT,
                           dpi = empleado.DPI,


                       });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> PaEliminarEmpleado(int empleadoId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(@"
                    EXEC paEliminarEmpleado @empleadoid",
                    new
                    {
                        empleadoid = empleadoId,
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
