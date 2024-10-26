using Microsoft.AspNetCore.Mvc;
using SistemaILP.comisariato.Models;
using SistemaILP.comisariato.Servicios;
using System.Diagnostics;

namespace SistemaILP.comisariato.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPermisosService _permisosService;
        private readonly IBreadcrumbService _breadcrumbService;

        public HomeController(ILogger<HomeController> logger, IPermisosService permisosService, IBreadcrumbService breadcrumbService)
        {
            _logger = logger;
            _permisosService = permisosService;
            _breadcrumbService = breadcrumbService;
        }

        public async Task<IActionResult> Index()
        {
            List<BreadcrumbItem> breadcrumbItems = _breadcrumbService.GetBreadcrumbItems(HttpContext);
            ViewBag.BreadcrumbItems = breadcrumbItems;

            bool esPermitido = await _permisosService.ValidaPermisoPrograma();

            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Error403()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
