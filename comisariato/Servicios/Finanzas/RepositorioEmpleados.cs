using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.Finanzas
{
    public interface IRepositorioEmpleado
    {
        Task<List<Empleados>> ObtieneTodoEmpleados();
        Task<Empleados> ObtienePorEmpleadoId(int id);
        Task<bool> PaEliminarEmpleado(int id);
    }
    public class RepositorioEmpleados : IRepositorioEmpleado
    {
        private readonly string _connectionString;

        public RepositorioEmpleados(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato")?? "";
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
