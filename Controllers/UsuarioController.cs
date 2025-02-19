using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Filters;
using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Repositories;
using tl2_proyecto_2024_nachoNota.Services;
using tl2_proyecto_2024_nachoNota.ViewModels.UsuarioVM;

namespace tl2_proyecto_2024_nachoNota.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<UsuarioController> _logger;
        public UsuarioController(IUsuarioRepository usuarioRepository, IAuthenticationService authenticationService, ILogger<UsuarioController> logger)
        {
            _usuarioRepository = usuarioRepository;
            _authenticationService = authenticationService;
            _logger = logger;
        }

        public ActionResult Registrar()
        {
            var usuarioVM = new RegistrarUsuarioViewModel();
            return View(usuarioVM);
        }

        [HttpPost]
        public ActionResult Registrar(RegistrarUsuarioViewModel usuarioVM)
        {
            var usuario = new Usuario(usuarioVM);

            _usuarioRepository.Create(usuario);
            return RedirectToAction("Index", "Login");
        }

        [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public IActionResult Modificar(int idUsuario)
        {
            var usuario = _usuarioRepository.GetById(idUsuario);
            var usuarioVM = new ModificarUsuarioViewModel(usuario);
            return View(usuarioVM);
        }

        [HttpPost]
        [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public IActionResult Modificar(ModificarUsuarioViewModel usuarioVM)
        {
            var usuario = new Usuario(usuarioVM);
            _usuarioRepository.Update(usuario);
            _authenticationService.ChangeUserName(usuario.NombreUsuario);

            TempData["Mensaje"] = "El usuario fue modificado con éxito.";

            return RedirectToAction("Modificar", new {idUsuario = usuario.Id});
        }

        [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public IActionResult Eliminar(int idUsuario)
        {
            var usuarioVM = new EliminarUsuarioViewModel(idUsuario);
            return View(usuarioVM);
        }

        [HttpPost]
        [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public IActionResult Eliminar(EliminarUsuarioViewModel usuarioVM)
        {
            bool ContraseniasIguales = usuarioVM.ContraseniaActual == usuarioVM.ContraseniaIngresada;
            if (ContraseniasIguales)
            {
                _usuarioRepository.Delete(usuarioVM.Id);
                TempData["Mensaje"] = "El usuario fue eliminado con éxito.";
                return RedirectToAction("Logout", "Login");
            }

            usuarioVM.ErrorMessage = "Las contraseñas no coinciden, asegúrate de escribir todo correctamente";

            return View(usuarioVM);
        }

        [AccessLevel(RolUsuario.Admin)]
        public IActionResult EliminarParaAdmin(int idUsuario)
        {
            var usuario = _usuarioRepository.GetById(idUsuario);
            var usuarioVM = new EliminarUsuarioViewModel(idUsuario, usuario.NombreUsuario);
            return View(usuarioVM);
        }

        [HttpPost]
        [AccessLevel(RolUsuario.Admin)]
        public IActionResult EliminarParaAdmin(EliminarUsuarioViewModel usuarioVM)
        {
            bool ContraseniasIguales = usuarioVM.ContraseniaActual == usuarioVM.ContraseniaIngresada;
            if (ContraseniasIguales)
            {
                _usuarioRepository.Delete(usuarioVM.Id);
                TempData["Mensaje"] = "El usuario fue eliminado con éxito";
                return RedirectToAction("Editar");
            }

            usuarioVM.ErrorMessage = "Las contraseñas no coinciden, asegúrate de escribir todo correctamente";

            return View(usuarioVM);
        }

        [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public IActionResult CambiarContra(int idUsuario)
        {
            var usuario = _usuarioRepository.GetById(idUsuario);
            var usuarioVM = new CambiarContraViewModel();
            usuarioVM.Id = usuario.Id;
            return View(usuarioVM);
        }

        [HttpPost]
        [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public IActionResult CambiarContra(CambiarContraViewModel usuarioVM)
        {
            bool contrasActualesIguales = usuarioVM.PasswordActual == HttpContext.Session.GetString("Password");

            if (contrasActualesIguales)
            {
                _usuarioRepository.ChangePassword(usuarioVM.Id, usuarioVM.PasswordNueva);
                _authenticationService.ChangePassword(usuarioVM.PasswordNueva);

                TempData["Mensaje"] = "La nueva contraseña ha sido guardada con éxito.";
                return RedirectToAction("CambiarContra", new {idUsuario = usuarioVM.Id});
            }

            usuarioVM.ErrorMessage = "La contraseña no coincide con la actual, asegúrate de escribirla correctamente";
            return View(usuarioVM);
        }

        [AccessLevel(RolUsuario.Admin)]
        public IActionResult Crear()
        {
            return View(new CrearUsuarioViewModel());
        }

        [HttpPost]
        [AccessLevel(RolUsuario.Admin)]
        public IActionResult Crear(CrearUsuarioViewModel usuarioVM)
        {
            var usuario = new Usuario(usuarioVM.Rol, usuarioVM.NombreUsuario, usuarioVM.Contrasenia, usuarioVM.Email);
            _usuarioRepository.Create(usuario);
            TempData["Mensaje"] = "El nuevo usuario fue creado con éxito.";
            return RedirectToAction("Crear");
        }
        
        [AccessLevel(RolUsuario.Admin)]
        public IActionResult Editar()
        {
            return View(new List<EditarUsuarioViewModel>());
        }

        [AccessLevel(RolUsuario.Admin)]
        public IActionResult CambiarRol(int idUsuario)
        {
            var usuario = _usuarioRepository.GetById(idUsuario);

            var usuarioVM = new CambiarRolViewModel(usuario.NombreUsuario, idUsuario);
            return View(usuarioVM);
        }

        [HttpPost]
        [AccessLevel(RolUsuario.Admin)]
        public IActionResult CambiarRol(CambiarRolViewModel usuarioVM)
        {
            bool contraseniasIguales = usuarioVM.ContraseniaActual == usuarioVM.ContraseniaIngresada;
            if (contraseniasIguales)
            {
                _usuarioRepository.ChangeRol(usuarioVM.Id, usuarioVM.Rol);
                _authenticationService.ChangeAccessLevel(usuarioVM.Rol);

                TempData["Mensaje"] = "El cambio de rol se ha producido de manera exitosa.";
                return RedirectToAction("Editar");
            }

            usuarioVM.ErrorMessage = "Las contraseñas no coinciden, asegúrate de escribir todo correctamente";

            return View(usuarioVM);
        }

        [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public IActionResult Buscar(string nombreUsuario)
        {
            var usuarios = _usuarioRepository.SearchByName(nombreUsuario).ToList();

            var usuariosVM = usuarios.Select(u => new EditarUsuarioViewModel(u.Id, u.NombreUsuario , u.Email, u.Rol)).ToList();

            return View("Editar", usuariosVM);
        }

        
    }
}
