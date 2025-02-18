using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Filters;
using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Repositories;
using tl2_proyecto_2024_nachoNota.ViewModels;
using tl2_proyecto_2024_nachoNota.ViewModels.TableroVM;

namespace tl2_proyecto_2024_nachoNota.Controllers
{
    [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
    public class TableroController : Controller
    {
        private readonly ITableroRepository _tableroRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public TableroController(ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository)
        {
            _tableroRepository = tableroRepository;
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult ListarPropios(int idUsuario)
        {
            var tableros = _tableroRepository.GetAllByUser(idUsuario).ToList();

            var tablerosVM = tableros.Select(t => new ListarTablerosPropiosViewModel(
                t.Id,
                t.Titulo,
                t.Color,
                t.Descripcion
                )).ToList();

            return View(tablerosVM);
        }

        public IActionResult ListarConTareasAsignadas(int idUsuario)
        {
            var tableros = _tableroRepository.GetTablerosConTareasAsignadas(idUsuario).ToList(); 

            var tablerosVM = tableros.Select(t => new ListarTablerosAjenosViewModel(
                t.Id,
                t.Titulo,
                t.Color,
                _usuarioRepository.GetNameById(t.IdUsuario))).ToList();

            return View(tablerosVM);
        }

        public IActionResult ListarBuscados(int idUsuario)
        {
            var tableros = _tableroRepository.GetAllByUser(idUsuario);

            var tablerosVM = tableros.Select(t => new ListarTablerosAjenosViewModel(
                t.Id,
                t.Titulo,
                t.Color,
                _usuarioRepository.GetNameById(t.IdUsuario))).ToList();

            return View(tablerosVM);
        }

        [HttpPost]
        public IActionResult Crear(CrearTableroViewModel tableroVM)
        {
            var tablero = new Tablero(tableroVM.Titulo, tableroVM.Color, tableroVM.Descripcion, tableroVM.IdUsuario);

            _tableroRepository.Create(tablero);
            TempData["Mensaje"] = "El tablero fue creado con éxito.";
            return RedirectToAction("ListarPropios", new {IdUsuario = tablero.IdUsuario });   
        }

        [HttpPost]
        public IActionResult Modificar(ModificarTableroViewModel tableroVM)
        {
            var tablero = new Tablero(tableroVM.Id, tableroVM.Titulo, tableroVM.Color, tableroVM.Descripcion);

            _tableroRepository.Update(tablero);
            TempData["Mensaje"] = "El tablero fue modificado con éxito.";
            return RedirectToAction("ListarPropios", new { IdUsuario = HttpContext.Session.GetInt32("IdUser") });
        }

        [HttpPost]
        public IActionResult Eliminar(int idTablero)
        {
            _tableroRepository.Delete(idTablero);
            TempData["Mensaje"] = "El tablero fue eliminado con éxito";
            return RedirectToAction("ListarPropios", new { IdUsuario = HttpContext.Session.GetInt32("IdUser") });
        }

        public IActionResult MostrarBuscadorUsuarios()
        {
            return View(new List<UsuarioBuscadoViewModel>());
        }

        public IActionResult BuscarUsu(string nombreUsuario)
        {
            var usuariosBuscados = _usuarioRepository.SearchByName(nombreUsuario);

            var usuariosVM = usuariosBuscados.Select(u => new UsuarioBuscadoViewModel(u.Id, u.NombreUsuario)).ToList();

            return View("MostrarBuscadorUsuarios", usuariosVM);
        }

    }
}
