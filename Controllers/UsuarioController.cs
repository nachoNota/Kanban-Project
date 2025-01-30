using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Repositories;
using tl2_proyecto_2024_nachoNota.Services;
using tl2_proyecto_2024_nachoNota.ViewModels;
using tl2_proyecto_2024_nachoNota.ViewModels.UsuarioVM;

namespace tl2_proyecto_2024_nachoNota.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuthenticationService _authenticationService;

        public UsuarioController(IUsuarioRepository usuarioRepository, IAuthenticationService authenticationService)
        {
            _usuarioRepository = usuarioRepository;
            _authenticationService = authenticationService;
        }

        public IActionResult Buscar(string nombreUsuario, int idTablero)
        {
            var usuarios = _usuarioRepository.SearchByName(nombreUsuario);
            var buscarVM = new BuscarViewModel();

            if(usuarios is null)
            {
                buscarVM.IdTablero = idTablero;
                return PartialView("Buscar", buscarVM);
            }

            List<BuscarUsuarioViewModel> usuariosVM = new List<BuscarUsuarioViewModel>();

            foreach (var usuario in usuarios)
            {
                var usuarioVM = new BuscarUsuarioViewModel(usuario.Id, usuario.NombreUsuario);
                usuariosVM.Add(usuarioVM);
            }

            buscarVM = new BuscarViewModel(idTablero, usuariosVM);

            return PartialView("Buscar", buscarVM);
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

        public IActionResult Modificar(int idUsuario)
        {
            var usuario = _usuarioRepository.GetById(idUsuario);
            var usuarioVM = new ModificarUsuarioViewModel(usuario);
            return View(usuarioVM);
        }

        [HttpPost]
        public IActionResult Modificar(ModificarUsuarioViewModel usuarioVM)
        {
            var usuario = new Usuario(usuarioVM);
            _usuarioRepository.Update(usuario);
            _authenticationService.ChangeUserName(usuario.NombreUsuario);

            return RedirectToAction("Listar", "Tablero");
        }

        public IActionResult Eliminar(int idUsuario)
        {
            var contrasenia = HttpContext.Session.GetString("Password");

            var usuarioVM = new EliminarUsuarioViewModel(idUsuario, contrasenia);
            return View(usuarioVM);
        }
        
        [HttpPost]
        public IActionResult Eliminar(EliminarUsuarioViewModel usuarioVM)
        {
            bool ContraseniasIguales = HttpContext.Session.GetString("Password") == usuarioVM.Password;
            if (ContraseniasIguales)
            {
                _usuarioRepository.Delete(usuarioVM.Id);
                return RedirectToAction("Index", "Login");
            }

            usuarioVM.ErrorMessage = "Las contraseñas no coinciden, asegúrate de escribir todo correctamente";

            return View(usuarioVM);
        }

        public IActionResult CambiarContra(int idUsuario)
        {
            var usuario = _usuarioRepository.GetById(idUsuario);
            var usuarioVM = new CambiarContraViewModel();
            usuarioVM.Id = usuario.Id;
            return View(usuarioVM);
        }

        [HttpPost]
        public IActionResult CambiarContra(CambiarContraViewModel usuarioVM)
        {
            bool contrasActualesIguales = usuarioVM.PasswordActual == HttpContext.Session.GetString("Password");

            if (contrasActualesIguales)
            {
                _usuarioRepository.ChangePassword(usuarioVM.Id, usuarioVM.PasswordNueva);
                return RedirectToAction("Index", "Login");
            }

            usuarioVM.ErrorMessage = "La contraseña no coincide con la actual, asegúrate de escribirla correctamente";
            return View(usuarioVM);
        }
    }
}
