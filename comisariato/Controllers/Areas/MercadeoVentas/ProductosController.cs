using Microsoft.AspNetCore.Authorization;
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

        public ProductosController(IPermisosService permisosService, IRepositorioProducto repositorioProducto)
        {
            _permisosService = permisosService;
            _repositorioProducto = repositorioProducto;
        }

        public async Task<IActionResult> Index(int? numpag)
        {
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
    }
}
