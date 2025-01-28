using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Repositories;

namespace tl2_proyecto_2024_nachoNota.Controllers
{
    public class TablaController : Controller
    {
        private readonly ITablaRepository _tablaRepository;

        public TablaController(ITablaRepository tablaRepository)
        {
            _tablaRepository = tablaRepository;
        }

        public IActionResult Index(int idTablero)
        {
            var tablas = _tablaRepository.GetByTablero(idTablero);

            return View(tablas);
        }
    }
}
