using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Repositories;
using tl2_proyecto_2024_nachoNota.ViewModels;

namespace tl2_proyecto_2024_nachoNota.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
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
    }
}
