using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
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
            return _tableroRepository.GetAllByUser(idUsuario)
                .Select(t => new ListarTablerosViewModel(
                    t.Id,
                    t.IdUsuario,
                    t.Titulo,
                    t.Color,
                    t.Descripcion
                )).ToList();
        }

        public IActionResult Listar(int idUsuario)
        {
            try
            {
                if (!_usuarioRepository.Exists(idUsuario))
                {
                    return RedirectToAction("NoEncontrado", "Error", new { mensaje = "El usuario solicitado no existe en nuestra base de datos." });
                }
            
                List<ListarTablerosViewModel> tablerosVM = ObtenerTablerosViewModel(idUsuario);
                return View(tablerosVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar listar tableros del usuario {idUsuario}.", idUsuario);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder al usuario seleccionado. Por favor, intente de nuevo más tarde." });
            }
        }

        [AccessLevel(RolUsuario.Admin)]
        public IActionResult ListarBuscados(int idUsuario)
        {
            try
            {
                if (!_usuarioRepository.Exists(idUsuario))
                {
                    return RedirectToAction("NoEncontrado", "Error", new { mensaje = "El usuario solicitado no existe en nuestra base de datos." });
                }

                List<ListarTablerosViewModel> tablerosVM = ObtenerTablerosViewModel(idUsuario);
                string nombreUsuario = _usuarioRepository.GetNameById(idUsuario);

                var datosTableros = new ListarTablerosBuscadosViewModel(nombreUsuario, tablerosVM);

                return View(datosTableros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar listar tableros del usuario {idUsuario}.", idUsuario);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder al usuario seleccionado. Por favor, intente de nuevo más tarde." });
            }
        }

        public IActionResult ListarConTareasAsignadas(int idUsuario)
        {
            try
            {
                if (!_usuarioRepository.Exists(idUsuario))
                {
                    return RedirectToAction("NoEncontrado", "Error", new { mensaje = "El usuario solicitado no existe en nuestra base de datos." });
                }

                var tableros = _tableroRepository.GetTablerosConTareasAsignadas(idUsuario).ToList(); 
                var tablerosVM = tableros.Select(t => new ListarTablerosAjenosViewModel(
                    t.Id,
                    t.Titulo,
                    t.Color,
                    _usuarioRepository.GetNameById(t.IdUsuario))).ToList();

                return View(tablerosVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar listar tableros del usuario {idUsuario}.", idUsuario);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder al usuario seleccionado. Por favor, intente de nuevo más tarde." });
            }
        }

        [HttpPost]
        public IActionResult Crear(CrearTableroViewModel tableroVM)
        {
            try
            {
                var tablero = new Tablero(tableroVM.Titulo, tableroVM.Color, tableroVM.Descripcion, tableroVM.IdUsuario);

                _tableroRepository.Create(tablero);
                TempData["Mensaje"] = "El tablero fue creado con éxito.";
                return RedirectToAction("Listar", new { tableroVM.IdUsuario });   
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar crear un tablero");
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar crear el tablero seleccionado. Por favor, intente de nuevo más tarde." });
            }
        }

        [HttpPost]
        public IActionResult Modificar(ModificarTableroViewModel tableroVM)
        {
            try
            {
                var tablero = new Tablero(tableroVM.Id, tableroVM.Titulo, tableroVM.Color, tableroVM.Descripcion);

                _tableroRepository.Update(tablero);
                TempData["Mensaje"] = "El tablero fue modificado con éxito.";

                bool esUrlValida = !string.IsNullOrEmpty(tableroVM.ReturnUrl) && Url.IsLocalUrl(tableroVM.ReturnUrl);
                if (esUrlValida)
                {
                    return Redirect(tableroVM.ReturnUrl);
                }
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar modificar el tablero {Id}.", tableroVM.Id);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar modificar el tablero seleccionado. Por favor, intente de nuevo más tarde." });
            }
        }

        [HttpPost]
        public IActionResult Eliminar(EliminarTableroViewModel tableroVM)
        {
            try
            {
                _tableroRepository.Delete(tableroVM.IdTablero);
                TempData["Mensaje"] = "El tablero fue eliminado con éxito";

                bool esUrlValida = !string.IsNullOrEmpty(tableroVM.ReturnUrl) && Url.IsLocalUrl(tableroVM.ReturnUrl);
                if(esUrlValida)
                {
                    return Redirect(tableroVM.ReturnUrl);
                }
                return RedirectToAction("Index", "Login");
            }
            catch (MySqlException ex) when (ex.Number == 1451) //error de restriccion de FK
            {
                TempData["Mensaje"] = "El tablero que se quiere borrar está relacionado con tareas, por lo que no puede ser eliminado.";
                return Redirect(tableroVM.ReturnUrl);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar eliminar el tablero {IdTablero}.", tableroVM.IdTablero);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar eliminar el tablero seleccionado. Por favor, intente de nuevo más tarde." });
            }
        }


        [AccessLevel(RolUsuario.Admin)]
        public IActionResult MostrarBuscadorUsuarios()
        {
            return View(new List<UsuarioBuscadoViewModel>());
        }

        [AccessLevel(RolUsuario.Admin)]
        public IActionResult BuscarUsu(string nombreUsuario)
        {
            try
            {
                var usuariosBuscados = _usuarioRepository.SearchByName(nombreUsuario);

                if(!usuariosBuscados.Any())
                {
                    TempData["Mensaje"] = $"No hemos encontrado coincidencias para '{nombreUsuario}', escribilo de otra forma y volvé a intentar.";
                }

                var usuariosVM = usuariosBuscados.Select(u => new UsuarioBuscadoViewModel(u.Id, u.NombreUsuario)).ToList();

                return View("MostrarBuscadorUsuarios", usuariosVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar buscar al usuario {nombreUsuario}.", nombreUsuario);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar buscar al usuario seleccionado. Por favor, intente de nuevo más tarde." });
            }
        }

    }
}
