using comisariato.Models;
using comisariato.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace comisariato.Controllers
{
    public class AuthController : Controller
    {
        private readonly IEncryptService _encryptService;
        private readonly IAuthService _authService;

        public AuthController(

            IEncryptService encryptService,
            IAuthService authService)
        {
            this._encryptService = encryptService;
            this._authService = authService;
        }

        //Get: Acceso
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuarios usuario)
        {
            usuario.UsuarioPassword = _encryptService.ConvertirSHA256(usuario.UsuarioPassword);

            try
            {
                usuario.UsuarioId = await _authService.Login(usuario);
               
            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }

            if (usuario.UsuarioId != 0)
            {
                // Si el login es exitoso, redirigir al usuario a la página de inicio
                return RedirectToAction("Index", "Home");

            }
            else
            {
                // Si el usuario no es encontrado o el ID es inválido, mostrar un error
                ViewData["Mensaje"] = "Usuario o Contraseña incorrectos.";
                return View();
            }
        }
    }
}
