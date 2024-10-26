using Microsoft.AspNetCore.Mvc;
using SistemaILP.comisariato.Data;
using SistemaILP.comisariato.Models;
using SistemaILP.comisariato.Servicios;
using SistemaILP.comisariato.Servicios.Sistemas;

namespace SistemaILP.comisariato.Controllers.Areas.Sistemas
{
    public class ComisariatosController : Controller
    {
        private readonly IPermisosService _permisosService;
        private readonly IRepositorioComisariatos _repositorioComisariatos;
        private readonly IBreadcrumbService _breadcrumbService;

        public ComisariatosController(IPermisosService permisosService, IRepositorioComisariatos repositorioComisariatos, IBreadcrumbService breadcrumbService)
        {
            _permisosService = permisosService;
            _repositorioComisariatos = repositorioComisariatos;
            _breadcrumbService = breadcrumbService;
        }

        [HttpGet]
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
                List<Comisariato> Listado = await _repositorioComisariatos.ObtieneTodoComisariatos();

                int cantidadregistros = 10;

                return View(Paginacion<Comisariato>.CrearPaginacion(Listado, numpag ?? 1, cantidadregistros));

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
