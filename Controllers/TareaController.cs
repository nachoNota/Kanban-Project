using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Repositories;
using tl2_proyecto_2024_nachoNota.ViewModels;

namespace tl2_proyecto_2024_nachoNota.Controllers
{
    public class TareaController : Controller
    {
        private readonly ITareaRepository _tareaRepository;
        private readonly ITableroRepository _tableroRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public TareaController(ITareaRepository tareaRepository, ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository)
        {
            _tareaRepository = tareaRepository;
            _tableroRepository = tableroRepository;
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Listar(int idTablero)
        {
            var tareas = _tareaRepository.GetByTablero(idTablero);
            var tablero = _tableroRepository.GetById(idTablero);

            var tareasVM = tareas.Select(t => new ListarTareasViewModel(t)).ToList();

            var tareasEnTableroVM = new ListarTareasDeTableroViewModel(tablero, tareasVM);
            return View(tareasEnTableroVM);
        }


        public IActionResult VerDetalles(int idTarea, int idPropietarioTablero)
        {
            var tarea = _tareaRepository.GetById(idTarea);
            var nombreUsuario = _usuarioRepository.GetNameById(tarea.IdUsuario);

            var tareaVM = new DetalleTareaViewModel(tarea, nombreUsuario, idPropietarioTablero);

            return View(tareaVM);
        }

        public IActionResult CambiarEstado(int idTarea, EstadoTarea estado)
        {
            _tareaRepository.CambiarEstado(idTarea, estado);
            return RedirectToAction("VerDetalles", new { idTarea , idPropietarioTablero = HttpContext.Session.GetInt32("IdUser") } );
        }

        public IActionResult Crear(int idTablero)
        {
            return View(new CrearTareaViewModel(idTablero));
        }

        [HttpPost]
        public IActionResult Crear(CrearTareaViewModel tareaVM)
        {
            var tarea = new Tarea(tareaVM.IdUsuario, tareaVM.IdTablero, tareaVM.Titulo, tareaVM.Descripcion, tareaVM.Color);
            _tareaRepository.Create(tarea);

            return RedirectToAction("Listar", new { idTablero = tareaVM.IdTablero });
        }

        [HttpPost]
        public IActionResult Eliminar(EliminarTareaViewModel tareaVM)
        {
            _tareaRepository.Delete(tareaVM.IdTarea);
            return RedirectToAction("Listar", new {idTablero = tareaVM.IdTablero});
        }

    }
}
