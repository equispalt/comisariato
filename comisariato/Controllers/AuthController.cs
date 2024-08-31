using comisariato.Models;
using comisariato.Servicios;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            catch (System.Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }

            if (usuario.UsuarioId != 0)
            {
                await SetSesion(usuario);
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

        private async Task SetSesion(Usuarios usuario)
        {
            List<Claim> sesion = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario.UsuarioName),  // agregar una claim con el nombre del usuario
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()) // agregar una claim con el id del usuario
            };

            ClaimsIdentity identidad = new(sesion, CookieAuthenticationDefaults.AuthenticationScheme); // crear identidad basada en los claims anteiores
            AuthenticationProperties autenticacionPropiedad = new(); //configurar persistencia
             
            autenticacionPropiedad.AllowRefresh = true; // permitir que la sesion se pueda refrescar
            autenticacionPropiedad.IsPersistent = true; // hacer que la sesion sea persistente
            autenticacionPropiedad.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1); // configurar fecha y hora de expiracion

            // iniciar sesion autenticada en el contexto HTTP usando cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidad), autenticacionPropiedad);
        }

    }
}
