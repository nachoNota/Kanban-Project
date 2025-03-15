using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using tl2_proyecto_2024_nachoNota.Filters;
using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Repositories;
using tl2_proyecto_2024_nachoNota.ViewModels;
using tl2_proyecto_2024_nachoNota.ViewModels.TareaVM;

namespace tl2_proyecto_2024_nachoNota.Controllers
{
    [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
    public class TareaController : Controller
    {
        private readonly ITareaRepository _tareaRepository;
        private readonly ITableroRepository _tableroRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<TareaController> _logger;

        public TareaController(ITareaRepository tareaRepository, ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository, ILogger<TareaController> logger) 
        {
            _tareaRepository = tareaRepository;
            _tableroRepository = tableroRepository;
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }
        /*
        public IActionResult Listar(int idTablero)
        {
            try
            {
                var tablero = _tableroRepository.GetById(idTablero);
                var tareas = _tareaRepository.GetByTablero(idTablero);
                var tareasVM = tareas.Select(t => new ListarTareasViewModel(t)).ToList();

                var tareasEnTableroVM = new ListarTareasDeTableroViewModel(tablero, tareasVM);

                return View(tareasEnTableroVM);
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction("NoEncontrado", "Error", new { mensaje = "El tablero solicitado no existe en nuestra base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar acceder al tablero {idTablero}.", idTablero);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder al tablero seleccionado. Por favor, intente de nuevo más tarde." });
            }
        }

        public IActionResult VerDetalles(int idTarea)
        {
            try
            {
                var tarea = _tareaRepository.GetById(idTarea);
                var nombreUsuario = string.Empty;

                if (tarea.IdUsuario != 0) nombreUsuario = _usuarioRepository.GetNameById(tarea.IdUsuario);

                int idPropietarioTablero = _tableroRepository.GetPropietario(tarea.IdTablero);
                var tareaVM = new DetalleTareaViewModel(tarea, nombreUsuario, idPropietarioTablero);

                return View(tareaVM);
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction("NoEncontrado", "Error", new { mensaje = "La tarea solicitada no existe en nuestra base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar acceder a la tarea {idTarea}.", idTarea);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder a la tarea seleccionada. Por favor, intente de nuevo más tarde." });
            }
        }

        public IActionResult CambiarEstado(int idTarea, EstadoTarea estado)
        {
            try
            {
                var tarea = _tareaRepository.GetById(idTarea);
                _tareaRepository.CambiarEstado(idTarea, estado);
                
                return RedirectToAction("VerDetalles", new { idTarea } );
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction("NoEncontrado", "Error", new { mensaje = "La tarea solicitada no existe en nuestra base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar acceder a la tarea {idTarea}.", idTarea);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder a la tarea seleccionada. Por favor, intente de nuevo más tarde." });
            }
        }

        public IActionResult Crear(int idTablero)
        {
            try
            {
                var tablero = _tableroRepository.GetById(idTablero);
            
                return View(new CrearTareaViewModel(idTablero));
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction("NoEncontrado", "Error", new { mensaje = "El tablero solicitado no existe en nuestra base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar acceder al tablero {idTablero}.", idTablero);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder al tablero seleccionado. Por favor, intente de nuevo más tarde." });
            }
        }

        [HttpPost]
        public IActionResult Crear(CrearTareaViewModel tareaVM)
        {
            try
            {
                var tarea = new Tarea(tareaVM.IdUsuario, tareaVM.IdTablero, tareaVM.Titulo, tareaVM.Descripcion, tareaVM.Color);
                _tareaRepository.Create(tarea);
                TempData["Mensaje"] = "La tarea fue creada con éxito";
    
                return RedirectToAction("Listar", new { idTablero = tareaVM.IdTablero });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar crear tarea");
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar crear la tarea seleccionada. Por favor, intente de nuevo más tarde." });
            }
        }

        [HttpPost]
        public IActionResult Eliminar(EliminarTareaViewModel tareaVM)
        {
            try
            {
                _tareaRepository.Delete(tareaVM.IdTarea);
                TempData["Mensaje"] = "La tarea fue eliminada con éxito.";

                return RedirectToAction("Listar", new { idTablero = tareaVM.IdTablero });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar eliminar tarea {IdTarea}", tareaVM.IdTarea);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar eliminar la tarea seleccionada. Por favor, intente de nuevo más tarde." });
            }

        }


        public IActionResult CambiarPropietarioTarea(int idTarea)
        {
            try
            {
                var tarea = _tareaRepository.GetById(idTarea);
                var cambiarPropietarioVM = new CambiarPropietarioTareaViewModel(idTarea, new List<UsuarioBuscadoViewModel>());

                return View(cambiarPropietarioVM);
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction("NoEncontrado", "Error", new { mensaje = "La tarea solicitada no existe en nuestra base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar acceder a la tarea {idTarea}.", idTarea);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder a la tarea seleccionada. Por favor, intente de nuevo más tarde." });
            }
        }

        [HttpPost]
        public IActionResult CambiarPropietarioTarea(int idTarea, int idUsuario)
        {
            try
            {
                _tareaRepository.AsignarUsuarioATarea(idUsuario, idTarea);
                TempData["Mensaje"] = "El usuario fue asignado a la tarea.";

                return RedirectToAction("VerDetalles", new { idTarea });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar acceder a la tarea {idTarea}.", idTarea);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder a la tarea seleccionada. Por favor, intente de nuevo más tarde." });
            }
        }

        public IActionResult Modificar(int idTarea)
        {
            try
            {
                var tarea = _tareaRepository.GetById(idTarea);
                var tareaVM = new ModificarTareaViewModel(tarea.Id, tarea.Titulo, tarea.Descripcion, tarea.Color);
            
                return View(tareaVM);
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction("NoEncontrado", "Error", new { mensaje = "La tarea solicitada no existe en nuestra base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar acceder a la tarea {idTarea}.", idTarea);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder a la tarea seleccionada. Por favor, intente de nuevo más tarde." });
            }
        }

        [HttpPost]
        public IActionResult Modificar(ModificarTareaViewModel tareaVM)
        {
            try
            {
                var tarea = new Tarea(tareaVM.Id, tareaVM.Titulo, tareaVM.Descripcion, tareaVM.Color);
                _tareaRepository.Update(tarea);

                TempData["Mensaje"] = "La tarea fue modificada con éxito.";
                return RedirectToAction("VerDetalles", new { idTarea = tareaVM.Id } );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar acceder a la tarea {idTarea}.", tareaVM.Id);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar modificar la tarea seleccionada. Por favor, intente de nuevo más tarde." });
            }
        }

        [HttpPost]
        public IActionResult BuscarUsu(string nombreUsuario, int idTarea)
        {
            try
            {
                var usuariosBuscados = _usuarioRepository.SearchByName(nombreUsuario).ToList();

                if (!usuariosBuscados.Any())
                {
                    TempData["Mensaje"] = $"No hemos encontrado coincidencias para '{nombreUsuario}', escribilo de otra forma y volvé a intentar.";
                }

                var usuariosBuscadosVM = usuariosBuscados.Select(u => new UsuarioBuscadoViewModel(u.Id, u.NombreUsuario)).ToList();
                var cambiarPropietarioVM = new CambiarPropietarioTareaViewModel(idTarea, usuariosBuscadosVM);

                return View("CambiarPropietarioTarea", cambiarPropietarioVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar buscar al usuario {nombreUsuario}.", nombreUsuario);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar buscar al usuario seleccionado. Por favor, intente de nuevo más tarde." });
            }
        }*/
    }
}
