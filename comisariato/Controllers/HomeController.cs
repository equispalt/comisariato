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

        public HomeController(ILogger<HomeController> logger, IPermisosService permisosService)
        {
            _logger = logger;
            _permisosService = permisosService;
        }

        public async Task<IActionResult> Index()
        {
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
