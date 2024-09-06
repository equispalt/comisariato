using Microsoft.AspNetCore.Mvc;
using SistemaILP.comisariato.Servicios;

namespace SistemaILP.comisariato.Controllers.Areas.Sistemas
{
    public class UsuariosController : Controller
    {
        private readonly IPermisosService _permisosService;

        public UsuariosController(IPermisosService permisosService) 
        { 
            this._permisosService = permisosService;
        } 
        public async Task<IActionResult> Index()
        {
            bool tienePermiso = await _permisosService.ValidaPermisoForm();

            if (!tienePermiso) 
            {
                return RedirectToAction("Error", "Home");
            }

            return View();
        }
    }
}
