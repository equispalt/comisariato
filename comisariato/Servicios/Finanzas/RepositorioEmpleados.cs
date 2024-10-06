using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.Finanzas
{
    public interface IRepositorioEmpleado
    {
        Task<List<Empleados>> ObtieneTodoEmpleados();
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

    }
}
