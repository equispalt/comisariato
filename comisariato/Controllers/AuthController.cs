using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SistemaILP.comisariato.Servicios;
using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Controllers
{
    public class AuthController : Controller
    {
        private readonly IEncryptService _encryptService;
        private readonly IAuthService _authService;


        public AuthController
            (
            IEncryptService encryptService,
            IAuthService authService
            )
        {
            _encryptService = encryptService;
            _authService = authService;
        }

        //Get: Acceso
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuarios usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.UsuarioName) || string.IsNullOrWhiteSpace(usuario.UsuarioPassword))
            {
                ViewData["Mensaje"] = "Ambos campos son obligatorios.";
                return View();
            }

            usuario.UsuarioPassword = _encryptService.ConvertirSHA256(usuario.UsuarioPassword);

            try
            {
                usuario.UsuarioId = await _authService.Login(usuario);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }

            if (usuario.UsuarioId != 0)
            {
                await _authService.SetSesion(usuario);
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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}
