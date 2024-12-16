using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Repositories;

namespace tl2_proyecto_2024_nachoNota.Controllers
{
    public class TareaController : Controller
    {
        private readonly ITareaRepository _tareaRepository;

        public TareaController(ITareaRepository tareaRepository)
        {
            _tareaRepository = tareaRepository;
        }

        public IActionResult Listar()
        {
            return View(_tareaRepository.GetAll());
        }

        public FileResult GetImagen(int id)
        {
            Tarea tarea = _tareaRepository.GetById(id);
            
            if(tarea.Imagen is null)
            {
                return null;
            }

            return File(tarea.Imagen, "image/jpg");
        }
    }
}
