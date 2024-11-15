﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaILP.comisariato.Data;
using SistemaILP.comisariato.Models;
using SistemaILP.comisariato.Servicios;
using SistemaILP.comisariato.Servicios.MercadeoVentas;

namespace SistemaILP.comisariato.Controllers.Areas.MercadeoVenta
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly IPermisosService _permisosService;
        private readonly IRepositorioProducto _repositorioProducto;
        private readonly IBreadcrumbService _breadcrumbService;

        public ProductosController(IPermisosService permisosService, IRepositorioProducto repositorioProducto, IBreadcrumbService breadcrumbService)
        {
            _permisosService = permisosService;
            _repositorioProducto = repositorioProducto;
            _breadcrumbService = breadcrumbService;
        }

        public async Task<IActionResult> Index(int? numpag)
        {
            List<BreadcrumbItem> breadcrumbItems = _breadcrumbService.GetBreadcrumbItems(HttpContext);
            ViewBag.BreadcrumbItems = breadcrumbItems;

            bool esPermitido = await _permisosService.ValidaPermisoPrograma();

            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }

            try
            {
                List<Productos> Listado = await _repositorioProducto.ObtieneTodoProductos();

                int cantidadregistros = 10;

                return View(Paginacion<Productos>.CrearPaginacion(Listado, numpag ?? 1, cantidadregistros));

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpGet]
        public async Task<JsonResult> ExisteCodigoProducto(string codigosilp)
        {
            bool existe = await _repositorioProducto.PaValidarCodigoProducto(codigosilp);
            return Json(new { existe });
        }

        [HttpGet]
        public async Task<JsonResult> ExisteCodigoBarraProducto(string codigobarra)
        {
            bool existe = await _repositorioProducto.PaValidarCodigoBarraProducto(codigobarra);
            return Json(new { existe });
        }

        [HttpPost]
        public async Task<IActionResult> CrearProducto(Productos newProducto)
        {
            bool esPermitido = await _permisosService.ValidaPermisoPrograma();
            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }
            try
            {

                bool creado = await _repositorioProducto.PaCrearProducto(newProducto);

                if (creado)
                {
                    return RedirectToAction("Index", "Productos");
                }
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditarProducto(int id, Productos updatedProd)
        {
            bool esPermitido = await _permisosService.ValidaPermisoPrograma();
            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }

            try
            {
                // Buscar el usuario por ID
                Productos producto = await _repositorioProducto.ObtienePorProductoId(id);

                if (producto == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                producto.Nombre = updatedProd.Nombre;
                producto.CodigoBarra = updatedProd.CodigoBarra;
                producto.Marca = updatedProd.Marca;
                producto.Categoria = updatedProd.Categoria;
                producto.Precio = updatedProd.Precio;

                // Guardar los cambios
                bool editado = await _repositorioProducto.PaEditarProducto(producto);

                if (editado)
                {
                    return RedirectToAction("Index", "Productos");
                }

                return RedirectToAction("Error", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            bool esPermitido = await _permisosService.ValidaPermisoPrograma();

            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }

            try
            {
                Productos producto = await _repositorioProducto.ObtienePorProductoId(id);
                if (producto == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                bool eliminado = await _repositorioProducto.PaEliminarProducto(id);

                if (eliminado)
                {
                    return RedirectToAction("Index", "Productos");
                }
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

        }


    }
}
