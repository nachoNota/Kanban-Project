using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Repositories;
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

        public ActionResult Listar()
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if(idUsuario is null)
            {
                return RedirectToAction("Index", "Login");
            }

            int usuario = idUsuario.Value;
            return View(_tableroRepository.GetAllByUser(usuario));
        }

        public ActionResult Modificar(int idTablero)
        {
            var tablero = _tableroRepository.GetById(idTablero);
            return PartialView(tablero);
        }

        [HttpPost]
        public ActionResult Modificar(Tablero tablero)
        {
            var tableroNuevo = new Tablero(tablero.Id, tablero.IdUsuario, tablero.Titulo, tablero.Color, tablero.Descripcion);
            _tableroRepository.Update(tableroNuevo);
            return RedirectToAction("Listar");
        }

        [HttpPost]
        public ActionResult Eliminar(int idTablero)
        {
            _tableroRepository.Delete(idTablero);
            return RedirectToAction("Listar");
        }

    }
}
