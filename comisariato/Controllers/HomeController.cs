using comisariato.Models;
using comisariato.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace comisariato.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPermisosService _permisosService;

        public HomeController(ILogger<HomeController> logger,IPermisosService permisosService)
        {
            _logger = logger;
            _permisosService = permisosService;
        }

        public async Task<IActionResult> Index()
        {
            bool tienePermiso = await _permisosService.ValidaPermisoForm();

            if (!tienePermiso) 
            { 
                return RedirectToAction("Error","Home");
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
    }
}
