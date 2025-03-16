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
        private readonly IPasswordService _passwordService;

        public LoginController(IAuthenticationService authenticationService, ILogger<LoginController> logger, IEmailService emailService,
            IUsuarioRepository usuarioRepository, IPasswordResetRepository passwordResetRepository, IPasswordService passwordService)
        {
            _authentication = authenticationService;
            _logger = logger;
            _emailService = emailService;
            _usuarioRepository = usuarioRepository;
            _passwordResetRepository = passwordResetRepository;
            _passwordService = passwordService;
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
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            try
            {
                bool loginExitoso = await _authentication.Login(loginVM.NombreUsuario, loginVM.Contrasenia);
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
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var usuario = await _usuarioRepository.GetByEmail(email);
            if (usuario is null)
            {
                TempData["Mensaje"] = "No se han encontrado coincidencias con el mail solicitado.";
                return View();
            }

            var passwordReset = new Passwordreset
            {
                IdUsuario = usuario.Id,
                Token = Guid.NewGuid().ToString(),
                Expiration = DateTime.Now.AddMinutes(5)
            };

            await _passwordResetRepository.Create(passwordReset);

            string resetLink = Url.Action("ResetPassword", "Login", new { token = passwordReset.Token }, Request.Scheme);

            await _emailService.SendEmail(email, "Recuperar contraseña", $"Haga click en el siguiente enlace para reestablecer su contraseña: " +
                                                                        $"<a href='{resetLink}'>Restablecer</a>");

            TempData["Mensaje"] = "Hemos enviado un enlace para reestablecer tu contraseña a tu correo.";
            return View();
        }

        public IActionResult ResetPassword(string token)
        {
            var passwordReset = new PasswordResetViewModel { Token = token };
            return View(passwordReset);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(PasswordResetViewModel passwordResetVM)
        {
            if(passwordResetVM.PasswordNueva != passwordResetVM.ConfirmPassword)
            {
                passwordResetVM.ErrorMessage = "Las contraseñas no coinciden, asegúrese de escribir todo correctamente.";
                return View(passwordResetVM);
            }

            var passwordReset = await _passwordResetRepository.GetByToken(passwordResetVM.Token);
            
            string HashedPassword = _passwordService.HashPassword(passwordResetVM.PasswordNueva);
            await _usuarioRepository.ChangePassword(passwordReset.IdUsuario, HashedPassword);
            await _passwordResetRepository.Delete(passwordReset.Id);

            TempData["Mensaje"] = "La contraseña se ha actualizado con éxito.";
            return RedirectToAction("Index");
        }


        public IActionResult Logout()
        {
            _authentication.Logout();
            return RedirectToAction("Index");
        }
    }
}
