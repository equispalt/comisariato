using Microsoft.AspNetCore.Mvc;
using SistemaILP.comisariato.Models;
using SistemaILP.comisariato.Servicios;
using SistemaILP.comisariato.Servicios.Sistemas;

namespace SistemaILP.comisariato.Controllers.Areas.Sistemas
{
    public class ProgramasController : Controller
    {
        private readonly IPermisosService _permisosService;
        private readonly IRepositorioProgramas _repositorioProgramas;

        public ProgramasController(IPermisosService permisosService, IRepositorioProgramas repositorioProgramas)
        {
            this._permisosService = permisosService;
            this._repositorioProgramas = repositorioProgramas;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            bool esPermitido = await _permisosService.ValidaPermisoPrograma();
            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }

            try
            {
                List<Programas> programa = await _repositorioProgramas.ObtieneTodoProgramas();

                return View(programa);

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

        }
    }
}
