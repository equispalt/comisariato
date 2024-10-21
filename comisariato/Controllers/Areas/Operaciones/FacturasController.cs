using Microsoft.AspNetCore.Mvc;
using SistemaILP.comisariato.Data;
using SistemaILP.comisariato.Models;
using SistemaILP.comisariato.Servicios;
using SistemaILP.comisariato.Servicios.Operaciones;

namespace SistemaILP.comisariato.Controllers.Areas.Operaciones
{
    public class FacturasController : Controller
    {
        private readonly IPermisosService _permisosService;
        private readonly IRepositorioFacturas _repositorioFacturas;

        public FacturasController(IPermisosService permisosService, IRepositorioFacturas repositorioFacturas)
        {
            _permisosService = permisosService;
            _repositorioFacturas = repositorioFacturas;
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
                List<FacVentas> Listado = await _repositorioFacturas.ObtieneTodoFacturas();

                int cantidadregistros = 10;

                return View(Paginacion<FacVentas>.CrearPaginacion(Listado, numpag ?? 1, cantidadregistros));

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
