using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
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
        private readonly IEncryptService _encryptService;
        private readonly IDatosDtoService _datosDtoService;

        public UsuariosController(IPermisosService permisosService, IRepositorioUsuario repositorioUsuario, IEncryptService encryptService, IDatosDtoService datosDtoService)
        {
            this._permisosService = permisosService;
            this._repositorioUsuario = repositorioUsuario;
            this._encryptService = encryptService;
            this._datosDtoService = datosDtoService;
        }

        //---------- Listar Usuario    ----------
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
                List<Estados> Estados = await _datosDtoService.ObtieneOpcionEstadoUsuario();
                ViewBag.Estados = Estados;

                List<Roles> Roles = await _datosDtoService.ObtieneTodoRoles();

                // Pasar los roles al ViewBag
                ViewBag.Roles = Roles;

                List<Empleados> Empleados = await _datosDtoService.ObtieneTodoEmpleados();
                ViewBag.Empleados = Empleados;


                List<Usuarios> Listado = await _repositorioUsuario.ObtieneTodoUsuarios();

                int cantidadregistros = 10;

                return View(Paginacion<Usuarios>.CrearPaginacion(Listado, numpag ?? 1, cantidadregistros));

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<JsonResult> ExisteUsuario(string usuario)
        {
            bool existe = await _repositorioUsuario.PaValidarUsuario(usuario);
            return Json(new { existe });
        }


        //---------- Crear Usuario    ----------
        [HttpPost]
        public async Task<IActionResult> CrearUsuario(Usuarios newUser)
        {
            bool esPermitido = await _permisosService.ValidaPermisoPrograma();
            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }
            try
            {
                newUser.Password = _encryptService.ConvertirSHA256(newUser.Password);

                bool creado = await _repositorioUsuario.PaCrearUsuario(newUser);

                if (creado)
                {
                    return RedirectToAction("Index", "Usuarios");
                }
                return RedirectToAction("Error", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error","Home");
            }
        }



        //---------- Editar Usuario    ----------
        [HttpPost]
        public async Task<IActionResult> EditarUsuario(int id, Usuarios updatedUser)
        {
            bool esPermitido = await _permisosService.ValidaPermisoPrograma();
            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }

            try
            {
                // Buscar el usuario por ID
                Usuarios usuario = await _repositorioUsuario.ObtienePorUsuarioId(id);

                if (usuario == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                usuario.EstadoId = updatedUser.EstadoId;

                // Guardar los cambios
                bool editado = await _repositorioUsuario.PaEditarUsuario(usuario);

                if (editado)
                {
                    return RedirectToAction("Index", "Usuarios");
                }

                return RedirectToAction("Error", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
        //---------- Editar contraseña    ----------
        [HttpPost]
        public async Task<IActionResult> EditarPassword(int id, string newPassword)
        {
            bool esPermitido = await _permisosService.ValidaPermisoPrograma();

            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }
            try
            {
                Usuarios usuario = await _repositorioUsuario.ObtienePorUsuarioId(id);

                if (usuario == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                usuario.Password = _encryptService.ConvertirSHA256(newPassword);

                bool actualizado = await _repositorioUsuario.PaEditarPassword(usuario);

                if (actualizado)
                {
                    return RedirectToAction("Index", "Usuarios");
                }

                return RedirectToAction("Error", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        //---------- Eliminar Usuario    ----------
        [HttpPost]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            bool esPermitido = await _permisosService.ValidaPermisoPrograma();

            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }

            try
            {
                Usuarios usuario = await _repositorioUsuario.ObtienePorUsuarioId(id);

                if (usuario == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                bool eliminado = await _repositorioUsuario.PaEliminarUsuario(id);

                if (eliminado)
                {
                    return RedirectToAction("Index", "Usuarios");
                }

                return RedirectToAction("Error", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
