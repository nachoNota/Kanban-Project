using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Repositories;

namespace tl2_proyecto_2024_nachoNota.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Buscar(string nombreUsuario)
        {
            var usuarios = _usuarioRepository.SearchByName(nombreUsuario);

            return View("", usuarios);
        }
    }
}
