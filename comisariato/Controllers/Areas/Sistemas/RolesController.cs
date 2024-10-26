using Microsoft.AspNetCore.Mvc;
using SistemaILP.comisariato.Data;
using SistemaILP.comisariato.Models;
using SistemaILP.comisariato.Servicios;
using SistemaILP.comisariato.Servicios.Sistemas;

namespace SistemaILP.comisariato.Controllers.Areas.Sistemas
{
    public class RolesController : Controller
    {
        private readonly IPermisosService _permisosService;
        private readonly IRepositorioRoles _repositorioRoles;
        private readonly IBreadcrumbService _breadcrumbService;

        public RolesController(IPermisosService permisosService, IRepositorioRoles repositorioRoles, IBreadcrumbService breadcrumbService)
        {
            this._permisosService = permisosService;
            this._repositorioRoles = repositorioRoles;
            this._breadcrumbService = breadcrumbService;
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
                List<Roles> Listado = await _repositorioRoles.ObtieneTodoRoles();

                int cantidadregistros = 10;

                return View(Paginacion<Roles>.CrearPaginacion(Listado, numpag ?? 1, cantidadregistros));

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }




    }
}
