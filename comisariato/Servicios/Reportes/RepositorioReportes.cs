using Dapper;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using OfficeOpenXml;

namespace SistemaILP.comisariato.Servicios.Reportes
{
    public interface IRepositorioReportes 
    {
        Task<List<FacVentas>> ResumenFacturas(DateTime inicio, DateTime fin);
        Task<List<FacVentas>> DetalleVentasPorProducto(DateTime inicio, DateTime fin);
        Task<List<TendenciaVentas>> TendenciaDeVentas(DateTime inicio, DateTime fin, bool AgruparPorAnio);
        byte[] GenerarExcelDesdeLista<T>(List<T> lista);
    }


    public class RepositorioReportes : IRepositorioReportes
    {
        private readonly string _connectionString;

        public RepositorioReportes(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato") ?? "";
        }

        public async Task<List<FacVentas>> ResumenFacturas(DateTime inicio, DateTime fin)
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<FacVentas> facturas = await connection.QueryAsync<FacVentas>(@"
                        EXEC resumenFacturasPorFecha  @FechaInicio, @FechaFin",
                        new { 
                            FechaInicio = inicio,
                            FechaFin = fin
                        });
            return facturas.ToList();
        }

        public async Task<List<FacVentas>> DetalleVentasPorProducto(DateTime inicio, DateTime fin)
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<FacVentas> facturas = await connection.QueryAsync<FacVentas>(@"
                        EXEC detalleVentasPorProducto  @FechaInicio, @FechaFin",
                        new
                        {
                            FechaInicio = inicio,
                            FechaFin = fin
                        });
            return facturas.ToList();
        }

        public async Task<List<TendenciaVentas>> TendenciaDeVentas(DateTime inicio, DateTime fin, bool AgruparPorAnio)
        {
            using var connection = new SqlConnection(_connectionString);
            IEnumerable<TendenciaVentas> facturas = await connection.QueryAsync<TendenciaVentas>(@"
                        EXEC TendenciaDeVentas  @FechaInicio, @FechaFin , @AgruparPorAnio",
                        new
                        {
                            FechaInicio = inicio,
                            FechaFin = fin,
                            AgruparPorAnio = AgruparPorAnio
                        });
            return facturas.ToList();
        }

        public byte[] GenerarExcelDesdeLista<T>(List<T> lista)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                // Crear una hoja de trabajo
                var hoja = package.Workbook.Worksheets.Add("Reporte");

                // Obtener las propiedades del tipo T
                var propiedades = typeof(T).GetProperties();

                // Añadir encabezados de las propiedades
                for (int i = 0; i < propiedades.Length; i++)
                {
                    hoja.Cells[1, i + 1].Value = propiedades[i].Name;
                }

                // Llenar datos
                for (int i = 0; i < lista.Count; i++)
                {
                    for (int j = 0; j < propiedades.Length; j++)
                    {
                        var valor = propiedades[j].GetValue(lista[i]);
                        hoja.Cells[i + 2, j + 1].Value = valor?.ToString();
                    }
                }

                // Guardar y retornar como un arreglo de bytes
                return package.GetAsByteArray();
            }
        }

    }
}
