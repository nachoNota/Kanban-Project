using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Repositories;
using tl2_proyecto_2024_nachoNota.Services;
using tl2_proyecto_2024_nachoNota.ViewModels;

namespace tl2_proyecto_2024_nachoNota.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthenticationService _authentication;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IAuthenticationService authenticationService, ILogger<LoginController> logger)
        {
            _authentication = authenticationService;
            _logger = logger;
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
            try
            {
                bool loginExitoso = _authentication.Login(loginVM.NombreUsuario, loginVM.Contrasenia);
                if (loginExitoso)
                {
                    return RedirectToAction("Listar", "Tablero", new { idUsuario = _authentication.GetUserId()});
                }

                loginVM.ErrorMessage = "Acceso inválido, asegúrese de escribir todo correctamente.";
                loginVM.IsAuthenticated = false;

            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar iniciar sesión.");
                ModelState.AddModelError("", "Se ha producido un error inesperado. Por favor, intente de nuevo mas tarde.");
            }
            return View("Index", loginVM);
        }

        public IActionResult Logout()
        {
            _authentication.Logout();
            return RedirectToAction("Index");
        }
    }
}
