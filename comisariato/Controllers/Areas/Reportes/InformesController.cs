using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using SistemaILP.comisariato.Data;
using SistemaILP.comisariato.Models;
using SistemaILP.comisariato.Servicios;
using SistemaILP.comisariato.Servicios.Reportes;
using System.Runtime.InteropServices;

namespace SistemaILP.comisariato.Controllers.Areas.Reportes
{
    public class InformesController : Controller
    {
        private readonly IPermisosService _permisosService;
        private readonly IBreadcrumbService _breadcrumbService;
        private readonly IRepositorioReportes _repositorioReportes;

        public InformesController(IPermisosService permisosService,IBreadcrumbService breadcrumbService, IRepositorioReportes repositorioReportes)
        {
            _permisosService = permisosService;
            _breadcrumbService = breadcrumbService;
            _repositorioReportes = repositorioReportes; 
        }
        public async Task<IActionResult> ResumenFacturas(DateTime? inicio, DateTime? fin)
        {
            List<BreadcrumbItem> breadcrumbItems = _breadcrumbService.GetBreadcrumbItems(HttpContext);
            ViewBag.BreadcrumbItems = breadcrumbItems;

            bool esPermitido = await _permisosService.ValidaPermisoPrograma();

            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }

            if (!inicio.HasValue || !fin.HasValue)
            {
                // Devuelve la vista vacía si no se han proporcionado las fechas
                return View();
            }

            try
            {

                List<FacVentas> Lista = await _repositorioReportes.ResumenFacturas(inicio.Value, fin.Value);

                return View(Lista);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }




    }
}
