using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Services;
using tl2_proyecto_2024_nachoNota.ViewModels;

namespace tl2_proyecto_2024_nachoNota.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthenticationService _authentication;

        public LoginController(IAuthenticationService authenticationService)
        {
            _authentication = authenticationService;
        }

        public IActionResult Index()
        {
            var modelo = new LoginViewModel
            {
                IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "true"
            };

            return View(modelo);
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginVM)
        {
            if (string.IsNullOrEmpty(loginVM.nombreUsuario) || string.IsNullOrEmpty(loginVM.contrasenia))
            {
                loginVM.ErrorMessage = "Debes completar los campos para continuar.";
                return View("Index", loginVM);
            }

            bool loginExitoso = _authentication.Login(loginVM.nombreUsuario, loginVM.contrasenia);
            if (loginExitoso)
            {
                return RedirectToAction("ListarPropios", "Tablero", new { idUsuario = HttpContext.Session.GetInt32("IdUser")});
            }

            loginVM.ErrorMessage = "Acceso inválido, asegúrese de escribir todo correctamente.";
            loginVM.IsAuthenticated = false;
            return View("Index", loginVM);
        }

        public IActionResult Logout()
        {
            _authentication.Logout();
            return RedirectToAction("Index");
        }
    }
}
