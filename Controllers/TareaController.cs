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

        public TareaController(ITareaRepository tareaRepository, ITableroRepository tableroRepository)
        {
            _tareaRepository = tareaRepository;
            _tableroRepository = tableroRepository;
        }

        public IActionResult Listar(int idTablero)
        {
            var tareas = _tareaRepository.GetByTablero(idTablero);
            var tablero = _tableroRepository.GetById(idTablero);

            var tareasVM = tareas.Select(t => new ListarTareasViewModel(t)).ToList();

            var tareasEnTableroVM = new ListarTareasDeTableroViewModel(tablero, tareasVM);
            return View(tareasEnTableroVM);
        }


        public IActionResult VerDetalles(int idTarea)
        {
            var tarea = _tareaRepository.GetById(idTarea);

            var tareaVM = new DetalleTareaViewModel(tarea);

            return View(tareaVM);
        }

    }
}
