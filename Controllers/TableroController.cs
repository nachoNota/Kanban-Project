using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Repositories;
using tl2_proyecto_2024_nachoNota.ViewModels.TableroVM;
using ZstdSharp.Unsafe;

namespace tl2_proyecto_2024_nachoNota.Controllers
{
    public class TableroController : Controller
    {
        private readonly ITableroRepository _tableroRepository;

        public TableroController(ITableroRepository tableroRepository)
        {
            _tableroRepository = tableroRepository;
        }

        public ActionResult ListarPropios(int idUsuario)
        {
            var tableros = _tableroRepository.GetAllByUser(idUsuario).ToList();
            return View(tableros);
        }

        public IActionResult ListarConTareasAsignadas(int idUsuario)
        {
            var tableros = _tableroRepository.GetTablerosConTareasAsignadas(idUsuario).ToList();

            return View(tableros);
        }

        [HttpPost]
        public ActionResult Crear(CrearTableroViewModel tableroVM)
        {
            var tablero = new Tablero(tableroVM.IdUsuario, tableroVM.Titulo, tableroVM.Color, tableroVM.Descripcion);

            _tableroRepository.Create(tablero);
            return RedirectToAction("ListarPropios", new {IdUsuario = tableroVM.IdUsuario});   
        }

        [HttpPost]
        public ActionResult Modificar(ModificarTableroViewModel tableroVM)
        {
            var tablero = new Tablero(tableroVM.Id, tableroVM.IdUsuario, tableroVM.Titulo, tableroVM.Color, tableroVM.Descripcion);

            _tableroRepository.Update(tablero);
            return RedirectToAction("ListarPropios", new { IdUsuario = tableroVM.IdUsuario });
        }

        [HttpPost]
        public ActionResult Eliminar(int idTablero)
        {
            _tableroRepository.Delete(idTablero);
            return RedirectToAction("ListarPropios", new { IdUsuario = HttpContext.Session.GetInt32("IdUser") });
        }

    }
}
