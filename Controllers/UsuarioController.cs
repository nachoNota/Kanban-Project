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
        private readonly IRolRepository _rolRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository, IAuthenticationService authenticationService, IRolRepository rolRepository)
        {
            _usuarioRepository = usuarioRepository;
            _authenticationService = authenticationService;
            _rolRepository = rolRepository;
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
        /*
        public IActionResult Buscar(BuscarViewModel buscarVM)
        {
            var usuarios = _usuarioRepository.SearchByName(buscarVM.Usuario);
            buscarVM.Usuarios = usuarios.ToList();
            return PartialView(buscarVM);
        }
        */
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
            var usuarioVM = new EliminarUsuarioViewModel(idUsuario);
            return View(usuarioVM);
        }

        [HttpPost]
        public IActionResult Eliminar(EliminarUsuarioViewModel usuarioVM)
        {
            bool ContraseniasIguales = usuarioVM.ContraseniaActual == usuarioVM.ContraseniaIngresada;
            if (ContraseniasIguales)
            {
                _usuarioRepository.Delete(usuarioVM.Id);
                return RedirectToAction("Logout", "Login");
            }

            usuarioVM.ErrorMessage = "Las contraseñas no coinciden, asegúrate de escribir todo correctamente";

            return View(usuarioVM);
        }

        public IActionResult EliminarParaAdmin(int idUsuario)
        {
            var usuario = _usuarioRepository.GetById(idUsuario);
            var usuarioVM = new EliminarUsuarioViewModel(idUsuario, usuario.NombreUsuario);
            return View(usuarioVM);
        }

        [HttpPost]
        public IActionResult EliminarParaAdmin(EliminarUsuarioViewModel usuarioVM)
        {
            bool ContraseniasIguales = usuarioVM.ContraseniaActual == usuarioVM.ContraseniaIngresada;
            if (ContraseniasIguales)
            {
                _usuarioRepository.Delete(usuarioVM.Id);
                return RedirectToAction("Editar");
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

        public IActionResult Crear()
        {
            var roles = _rolRepository.GetAll();
            return View(new CrearUsuarioViewModel(roles));
        }

        [HttpPost]
        public IActionResult Crear(CrearUsuarioViewModel usuarioVM)
        {
            var usuario = new Usuario(usuarioVM.IdRol, usuarioVM.NombreUsuario, usuarioVM.Contrasenia, usuarioVM.Email);
            _usuarioRepository.Create(usuario);
            return RedirectToAction("Crear");
        }

        public IActionResult Editar()
        {
            return View(new List<Usuario>());
        }

        public IActionResult CambiarRol(int idUsuario)
        {
            var usuario = _usuarioRepository.GetById(idUsuario);
            var roles = _rolRepository.GetAll();

            var usuarioVM = new CambiarRolViewModel(roles, usuario.NombreUsuario, idUsuario);
            return View(usuarioVM);
        }

        [HttpPost]
        public IActionResult CambiarRol(CambiarRolViewModel usuarioVM)
        {
            bool contraseniasIguales = usuarioVM.ContraseniaActual == usuarioVM.ContraseniaIngresada;
            if (contraseniasIguales)
            {
                _usuarioRepository.ChangeRol(usuarioVM.Id, usuarioVM.IdRol);
                return RedirectToAction("Editar");
            }

            usuarioVM.ErrorMessage = "Las contraseñas no coinciden, asegúrate de escribir todo correctamente";

            return View(usuarioVM);
        }

        public IActionResult Buscar(string nombreUsuario)
        {
            var usuarios = _usuarioRepository.SearchByName(nombreUsuario).ToList();
            return View("Editar", usuarios);
        }
    }
}
