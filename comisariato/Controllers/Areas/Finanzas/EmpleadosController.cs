using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SistemaILP.comisariato.Data;
using SistemaILP.comisariato.Models;
using SistemaILP.comisariato.Servicios;
using SistemaILP.comisariato.Servicios.Finanzas;
using System.Data.SqlTypes;

namespace SistemaILP.comisariato.Controllers.Areas.Finanzas
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        private readonly IPermisosService _permisosService;
        private readonly IRepositorioEmpleado _repositorioEmpleado;

        public EmpleadosController(IPermisosService permisosService, IRepositorioEmpleado repositorioEmpleado)
        {
            this._permisosService = permisosService;
            this._repositorioEmpleado = repositorioEmpleado;
        }

        public async Task<IActionResult> Index(int? numpag)
        {
            bool esPermitido = await _permisosService.ValidaPermisoPrograma();

            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }

            try
            {
                List<Empleados> Listado = await _repositorioEmpleado.ObtieneTodoEmpleados();

                int cantidadregistros = 10;

                return View(Paginacion<Empleados>.CrearPaginacion(Listado, numpag ?? 1, cantidadregistros));

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EliminarEmpleado(int id)
        {
            bool esPermitido = await _permisosService.ValidaPermisoPrograma();
            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }

            try
            {
                Empleados empleado = await _repositorioEmpleado.ObtienePorEmpleadoId(id);
                if (empleado == null)
                {
                    return RedirectToAction("Error","Home");
                }

                bool eliminado =await _repositorioEmpleado.PaEliminarEmpleado(id);

                if (eliminado)
                {
                    return RedirectToAction("Index","Empleados");
                }

                return RedirectToAction("Error","Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error","Home");
            }

        }

    }
}
