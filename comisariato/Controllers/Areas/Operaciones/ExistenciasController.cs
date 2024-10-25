using Microsoft.AspNetCore.Mvc;
using SistemaILP.comisariato.Data;
using SistemaILP.comisariato.Models;
using SistemaILP.comisariato.Servicios;
using SistemaILP.comisariato.Servicios.Operaciones;

namespace SistemaILP.comisariato.Controllers.Areas.Operaciones
{
    public class ExistenciasController : Controller
    {
        private readonly IPermisosService _permisosService;
        private readonly IRepositorioExistencias _repositorioExistencias;

        public ExistenciasController(IPermisosService permisosService, IRepositorioExistencias repositorioExistencias)
        {
            _permisosService = permisosService;
            _repositorioExistencias = repositorioExistencias;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? numpag)
        {
            bool esPermitido = await _permisosService.ValidaPermisoPrograma();
            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }

            try
            {
                List<Existencias> Listado = await _repositorioExistencias.ObtieneTodoExistencia();

                int cantidadregistros = 10;

                return View(Paginacion<Existencias>.CrearPaginacion(Listado, numpag ?? 1, cantidadregistros));

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

    }
}