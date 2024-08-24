using Microsoft.AspNetCore.Mvc;

namespace comisariato.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }
}
