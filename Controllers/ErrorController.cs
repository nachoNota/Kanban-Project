using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace tl2_proyecto_2024_nachoNota.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult ErrorInesperado(string mensaje)
        {
            ViewData["Mensaje"] = mensaje;
            return View();
        }
        
        public IActionResult NoEncontrado(string mensaje)
        {
            ViewData["Mensaje"] = mensaje;
            return View();
        }

        public IActionResult ErrorRol()
        {
            return View();
        }
    }
}
