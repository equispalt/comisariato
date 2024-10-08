﻿using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios.MercadeoVentas
{
    public interface IRepositorioProducto
    {
        Task<List<Productos>> ObtieneTodoProductos();
        Task<Productos> ObtinePorProductoId(int id);
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

        public async Task<Productos> ObtinePorProductoId(int id)
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
