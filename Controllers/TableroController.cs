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
        private async Task<List<ListarTablerosViewModel>> ObtenerTablerosViewModel(int idUsuario)
        {
            var tableros = await _tableroRepository.GetAllByUser(idUsuario);

            return tableros.Select(t => new ListarTablerosViewModel(
                    t.Id,
                    t.IdUsuario,
                    t.Titulo,
                    t.Color,
                    t.Descripcion
                )).ToList();
        }

        public async Task<IActionResult> Listar(int idUsuario)
        {
            try
            {
                if (!await _usuarioRepository.Exists(idUsuario))
                {
                    return RedirectToAction("NoEncontrado", "Error", new { mensaje = "El usuario solicitado no existe en nuestra base de datos." });
                }
            
                List<ListarTablerosViewModel> tablerosVM = await ObtenerTablerosViewModel(idUsuario);
                return View(tablerosVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar listar tableros del usuario {idUsuario}.", idUsuario);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder al usuario seleccionado. Por favor, intente de nuevo más tarde." });
            }
        }

        [AccessLevel(RolUsuario.Admin)]
        public async Task<IActionResult> ListarBuscados(int idUsuario)
        {
            try
            {
                if (!await _usuarioRepository.Exists(idUsuario))
                {
                    return RedirectToAction("NoEncontrado", "Error", new { mensaje = "El usuario solicitado no existe en nuestra base de datos." });
                }

                List<ListarTablerosViewModel> tablerosVM = await ObtenerTablerosViewModel(idUsuario);
                string nombreUsuario = await _usuarioRepository.GetNameById(idUsuario);

                var datosTableros = new ListarTablerosBuscadosViewModel(nombreUsuario, tablerosVM);

                return View(datosTableros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar listar tableros del usuario {idUsuario}.", idUsuario);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder al usuario seleccionado. Por favor, intente de nuevo más tarde." });
            }
        }

        public async Task<IActionResult> ListarConTareasAsignadas(int idUsuario)
        {
            try
            {
                if (!await _usuarioRepository.Exists(idUsuario))
                {
                    return RedirectToAction("NoEncontrado", "Error", new { mensaje = "El usuario solicitado no existe en nuestra base de datos." });
                }

                var tableros = await _tableroRepository.GetTablerosConTareasAsignadas(idUsuario);
                var tablerosVM = tableros.Select(t => new ListarTablerosAjenosViewModel
                {
                    IdTablero = t.Id,
                    Titulo = t.Titulo,
                    Color = t.Color,
                    NombrePropietario = t.IdUsuarioNavigation.NombreUsuario.ToString()
                }).ToList();

                return View(tablerosVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar listar tableros del usuario {idUsuario}.", idUsuario);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder al usuario seleccionado. Por favor, intente de nuevo más tarde." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearTableroViewModel tableroVM)
        {
            try
            {
                var tablero = new Tablero
                {
                    Titulo = tableroVM.Titulo,
                    Color = tableroVM.Color,
                    Descripcion = tableroVM.Descripcion,
                    IdUsuario = tableroVM.IdUsuario
                };

                await _tableroRepository.Create(tablero);
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
        public async Task<IActionResult> Modificar(ModificarTableroViewModel tableroVM)
        {
            try
            {
                var tablero = new Tablero 
                { 
                    Id = tableroVM.Id,
                    IdUsuario = tableroVM.IdUsuario,
                    Titulo = tableroVM.Titulo,
                    Color = tableroVM.Color,
                    Descripcion = tableroVM.Descripcion 
                };

                await _tableroRepository.Update(tablero);
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
        public async Task<IActionResult> Eliminar(EliminarTableroViewModel tableroVM)
        {
            try
            {
                await _tableroRepository.Delete(tableroVM.IdTablero);
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
        public async Task<IActionResult> BuscarUsu(string nombreUsuario)
        {
            try
            {
                var usuariosBuscados = await _usuarioRepository.SearchByName(nombreUsuario);

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
