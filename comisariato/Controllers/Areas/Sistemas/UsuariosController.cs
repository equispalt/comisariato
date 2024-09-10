using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaILP.comisariato.Data;
using SistemaILP.comisariato.Models;
using SistemaILP.comisariato.Servicios;
using SistemaILP.comisariato.Servicios.Sistemas;

namespace SistemaILP.comisariato.Controllers.Areas.Sistemas
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly IPermisosService _permisosService;
        private readonly IRepositorioUsuario _repositorioUsuario;

        public UsuariosController(IPermisosService permisosService, IRepositorioUsuario repositorioUsuario) 
        { 
            this._permisosService = permisosService;
            this._repositorioUsuario = repositorioUsuario;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? numpag)
        {
            bool esPermitido = await _permisosService.ValidaPermisoForm();

            if (esPermitido == false) 
            {
                return RedirectToAction("Error403", "Home");
            }

            try
            {
                List<Usuarios> Listado = await _repositorioUsuario.ObtieneTodoUsuarios();

                int cantidadregistros = 10;

                return View(Paginacion<Usuarios>.CrearPaginacion(Listado, numpag ?? 1, cantidadregistros));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
