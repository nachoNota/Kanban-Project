using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Repositories;
using tl2_proyecto_2024_nachoNota.Services;
using tl2_proyecto_2024_nachoNota.ViewModels;

namespace tl2_proyecto_2024_nachoNota.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthenticationService _authentication;
        private readonly ILogger<LoginController> _logger;
        private readonly IEmailService _emailService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordResetRepository _passwordResetRepository;

        public LoginController(IAuthenticationService authenticationService, ILogger<LoginController> logger, IEmailService emailService,
            IUsuarioRepository usuarioRepository, IPasswordResetRepository passwordResetRepository)
        {
            _authentication = authenticationService;
            _logger = logger;
            _emailService = emailService;
            _usuarioRepository = usuarioRepository;
            _passwordResetRepository = passwordResetRepository;
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

                return View("Index", loginVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar loguearse");
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar loguearse. Por favor, intente de nuevo más tarde." });
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            if (!_usuarioRepository.ExistsByEmail(email))
            {
                TempData["Mensaje"] = "No se han encontrado coincidencias con el mail solicitado.";
                return View();
            }

            var passwordReset = new PasswordReset(email);
            _passwordResetRepository.Create(passwordReset);

            string resetLink = Url.Action("ResetPassword", "Login", new { token = passwordReset.Token }, Request.Scheme);

            _emailService.SendEmail(email, "Recuperar contraseña", $"Haga click en el siguiente enlace para reestablecer su contraseña: " +
                                                                        $"<a href='{resetLink}'>Restablecer</a>");

            TempData["Mensaje"] = "Hemos enviado un enlace para reestablecer tu contraseña a tu correo.";
            return View();
        }

        public IActionResult ResetPassword(string token)
        {

        }


        public IActionResult Logout()
        {
            _authentication.Logout();
            return RedirectToAction("Index");
        }
    }
}
