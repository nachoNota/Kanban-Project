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
        private readonly ILogger<TableroController> _logger;

        public TableroController(ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository, ILogger<TableroController> logger)
        {
            _tableroRepository = tableroRepository;
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }
        private List<ListarTablerosViewModel> ObtenerTablerosViewModel(int idUsuario)
        {
            var tableros = _tableroRepository.GetAllByUser(idUsuario).ToList();

            var tablerosVM = tableros.Select(t => new ListarTablerosViewModel(
                t.Id,
                t.Titulo,
                t.Color,
                t.Descripcion
                )).ToList();
            return tablerosVM;
        }

        public IActionResult Listar(int idUsuario)
        {
            List<ListarTablerosViewModel> tablerosVM = ObtenerTablerosViewModel(idUsuario);
            return View(tablerosVM);
        }

        public IActionResult ListarBuscados(int idUsuario)
        {
            List<ListarTablerosViewModel> tablerosVM = ObtenerTablerosViewModel(idUsuario);
            string nombreUsuario = _usuarioRepository.GetNameById(idUsuario);

            var datosTableros = new ListarTablerosBuscadosViewModel(nombreUsuario, tablerosVM);

            return View(datosTableros);
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

        [HttpPost]
        public IActionResult Crear(CrearTableroViewModel tableroVM)
        {
            var tablero = new Tablero(tableroVM.Titulo, tableroVM.Color, tableroVM.Descripcion, tableroVM.IdUsuario);

            _tableroRepository.Create(tablero);
            TempData["Mensaje"] = "El tablero fue creado con éxito.";
            return RedirectToAction("Listar", new {IdUsuario = tablero.IdUsuario });   
        }

        [HttpPost]
        public IActionResult Modificar(ModificarTableroViewModel tableroVM)
        {
            var tablero = new Tablero(tableroVM.Id, tableroVM.Titulo, tableroVM.Color, tableroVM.Descripcion);

            _tableroRepository.Update(tablero);
            TempData["Mensaje"] = "El tablero fue modificado con éxito.";
            
            if (tableroVM.IdUsuario == HttpContext.Session.GetInt32("IdUser"))
            {
                return RedirectToAction("Listar", new { tableroVM.IdUsuario });
            }

            return RedirectToAction("ListarBuscados", new { tableroVM.IdUsuario });
        }

        [HttpPost]
        public IActionResult Eliminar(EliminarTableroViewModel tableroVM)
        {
            _tableroRepository.Delete(tableroVM.IdTablero);
            TempData["Mensaje"] = "El tablero fue eliminado con éxito";

            if (tableroVM.IdUsuario == HttpContext.Session.GetInt32("IdUser"))
            {
                return RedirectToAction("Listar", new { tableroVM.IdUsuario });
            }

            return RedirectToAction("ListarBuscados", new { tableroVM.IdUsuario });
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
