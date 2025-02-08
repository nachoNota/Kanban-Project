using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Repositories;
using tl2_proyecto_2024_nachoNota.ViewModels;
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
            int? idUsuario = HttpContext.Session.GetInt32("IdUser");

            if(idUsuario is null)
            {
                return RedirectToAction("Index", "Login");
            }

            int usuario = idUsuario.Value;
            return View(_tableroRepository.GetAllByUser(usuario));
        }

        public ActionResult Crear()
        {
            return View(new Tablero());
        }

        [HttpPost]
        public ActionResult Crear(Tablero tablero)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUser");

            if (idUsuario is null)
            {
                return RedirectToAction("Index", "Login");
            }

            tablero.IdUsuario = idUsuario.Value;
            _tableroRepository.Create(tablero);
            return RedirectToAction("Listar");   
        }

        public ActionResult Modificar(int idTablero)
        {
            var tablero = _tableroRepository.GetById(idTablero);
            var tableroVM = new TableroModificar(tablero);
            return PartialView(tableroVM);
        }

        [HttpPost]
        public ActionResult Modificar(TableroModificar tableroVM)
        {
            var tablero = new Tablero(tableroVM.Id, tableroVM.IdUsuario, tableroVM.Titulo, tableroVM.Color, tableroVM.Descripcion);

            _tableroRepository.Update(tablero);
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
