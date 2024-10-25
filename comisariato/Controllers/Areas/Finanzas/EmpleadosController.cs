using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaILP.comisariato.Data;
using SistemaILP.comisariato.Models;
using SistemaILP.comisariato.Servicios;
using SistemaILP.comisariato.Servicios.Finanzas;

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

        [HttpGet]
        public async Task<JsonResult> ExisteCodigoEmpleado(string codigo)
        {
            bool existe = await _repositorioEmpleado.PaValidarCodigoEmpleado(codigo);
            return Json(new { existe });
        }

        [HttpPost]
        public async Task<IActionResult> CrearEmpleado(Empleados newEmp)
        {
            bool esPermitido = await _permisosService.ValidaPermisoPrograma();
            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }
            try
            {

                bool creado = await _repositorioEmpleado.PaCrearEmpleado(newEmp);

                if (creado)
                {
                    return RedirectToAction("Index", "Empleados");
                }
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditarEmpleado(int id, Empleados updatedEmp)
        {
            bool esPermitido = await _permisosService.ValidaPermisoPrograma();
            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }

            try
            {
                // Buscar el usuario por ID
                Empleados empleado = await _repositorioEmpleado.ObtienePorEmpleadoId(id);

                if (empleado == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                empleado.Nombre = updatedEmp.Nombre;
                empleado.NIT = updatedEmp.NIT;
                empleado.DPI = updatedEmp.DPI;


                // Guardar los cambios
                bool editado = await _repositorioEmpleado.PaEditarEmpleado(empleado);

                if (editado)
                {
                    return RedirectToAction("Index", "Empleados");
                }

                return RedirectToAction("Error", "Home");
            }
            catch (Exception)
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
                    return RedirectToAction("Error", "Home");
                }

                bool eliminado = await _repositorioEmpleado.PaEliminarEmpleado(id);

                if (eliminado)
                {
                    return RedirectToAction("Index", "Empleados");
                }

                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

        }

    }
}
