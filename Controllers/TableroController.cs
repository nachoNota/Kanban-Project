using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Repositories;

namespace tl2_proyecto_2024_nachoNota.Controllers
{
    public class TableroController : Controller
    {
        private readonly ITableroRepository _tableroRepository;

        public TableroController(ITableroRepository tableroRepository)
        {
            _tableroRepository = tableroRepository;
        }

        public ActionResult Listar()
        {
            return View(_tableroRepository.GetAll());
        }

    }
}
