using Microsoft.AspNetCore.Mvc;
using SistemaILP.comisariato.Data;
using SistemaILP.comisariato.Servicios;
using SistemaILP.comisariato.Servicios.Sistemas;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Controllers.Areas.Sistemas
{
    public class RolesController : Controller
    {
        private readonly IPermisosService _permisosService;
        private readonly IRepositorioRoles _repositorioRoles;

        public RolesController(IPermisosService permisosService, IRepositorioRoles repositorioRoles)
        {
            this._permisosService = permisosService;
            this._repositorioRoles = repositorioRoles;
        }   

        [HttpGet]
        public async Task<IActionResult> Index(int? numpag)
        {
            bool esPermitido = await _permisosService.ValidaPermisoPrograma();
            if (esPermitido) 
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
