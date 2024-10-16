using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.MercadeoVentas
{
    public interface IRepositorioProducto
    {
        Task<List<Productos>> ObtieneTodoProductos();
        Task<Productos> ObtienePorProductoId(int id);
        Task<bool> PaValidarCodigoProducto(string codigosilp);
        Task<bool> PaValidarCodigoBarraProducto(string codigobarra);
        Task<bool> PaCrearProducto(Productos producto);
        Task<bool> PaEditarProducto(Productos producto);
        Task<bool> PaEliminarProducto(int id);
    }
    public class RepositorioProductos : IRepositorioProducto
    {
        private readonly string _connectionString;


        public RepositorioProductos(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato") ?? "";
        }

        public async Task<List<Productos>> ObtieneTodoProductos()
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<Productos> pro = await connection.QueryAsync<Productos>(@"
                    EXEC obtieneTodoProducto
            ");

            return pro.ToList();
        }

        public async Task<Productos> ObtienePorProductoId(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<Productos> pro = await connection.QueryAsync<Productos>(@"
                    EXEC obtieneProductoPorId @productoid", 
            new 
            { 
                productoid = id           
            });

            return pro.FirstOrDefault();
        }

        public async Task<bool> PaValidarCodigoProducto(string codigosilp)
        {
            using var connection = new SqlConnection(_connectionString);
            var existe = await connection.ExecuteScalarAsync<bool>(@"
            EXEC paValidarCodigoProducto @codigosilp",
            new { codigosilp });
            return existe;
        }

        public async Task<bool> PaValidarCodigoBarraProducto(string codigobarra)
        {
            using var connection = new SqlConnection(_connectionString);
            var existe = await connection.ExecuteScalarAsync<bool>(@"
            EXEC paValidarCodigoBarraProducto @codigobarra",
            new { codigobarra });
            return existe;
        }

        public async Task<bool> PaCrearProducto(Productos producto)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(@"
                EXEC paCrearProducto @codigosilp, @nombre, @codigobarra, @descripcion, @marca, @categoria, @precio ",
                new
                {
                    codigosilp = producto.CodigoSILP,
                    nombre = producto.Nombre,
                    codigobarra = producto.CodigoBarra,
                    descripcion = producto.Descripcion,
                    marca = producto.Marca,
                    categoria = producto.Categoria,
                    precio = producto.Precio, 

                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> PaEditarProducto(Productos producto)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(@"
                       EXEC paEditarProducto @productoid, @nombre, @codigobarra, @marca, @categoria, @precio ",
                       new
                       {
                           productoid = producto.ProductoId,
                           nombre = producto.Nombre,
                           codigobarra = producto.CodigoBarra,
                           marca = producto.Marca,
                           categoria = producto.Categoria,
                           precio = producto.Precio,

                       });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> PaEliminarProducto(int productoId)
        {
            try
            {
                using var connection = new SqlConnection (_connectionString);
                await connection.ExecuteAsync(@"
                    EXEC paEliminarProducto @productoid",
                    new 
                    { 
                        productoid = productoId
                    });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
