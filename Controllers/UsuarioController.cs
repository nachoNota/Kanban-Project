using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using tl2_proyecto_2024_nachoNota.Filters;
using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Repositories;
using tl2_proyecto_2024_nachoNota.Services;
using tl2_proyecto_2024_nachoNota.ViewModels.UsuarioVM;

namespace tl2_proyecto_2024_nachoNota.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<UsuarioController> _logger;
        public UsuarioController(IUsuarioRepository usuarioRepository, IAuthenticationService authenticationService,
            ILogger<UsuarioController> logger, IPasswordService passwordService)
        {
            _usuarioRepository = usuarioRepository;
            _authenticationService = authenticationService;
            _logger = logger;
            _passwordService = passwordService;
        }

        private bool VerificarContrasenia(int idUsuario, string contraseniaIngresada)
        {
            string contraseniaActual = _usuarioRepository.GetPasswordById(idUsuario);
            bool sonIguales = _passwordService.VerifyPassword(contraseniaActual, contraseniaIngresada);
            return sonIguales;
        }

        public ActionResult Registrar()
        {
            var usuarioVM = new RegistrarUsuarioViewModel();
            return View(usuarioVM);
        }

        [HttpPost]
        public ActionResult Registrar(RegistrarUsuarioViewModel usuarioVM)
        {
            try
            {
			    var usuario = new Usuario(usuarioVM.NombreUsuario, usuarioVM.Email, _passwordService.HashPassword(usuarioVM.Contrasenia), RolUsuario.Operador);

                _usuarioRepository.Create(usuario);
                TempData["Mensaje"] = "Usuario creado con éxito, prueba a iniciar sesión para confirmar los cambios.";
                return RedirectToAction("Index", "Login");
			}
            catch(MySqlException ex) when (ex.Number == 1062) // clave duplicada
            {
                ModelState.AddModelError("NombreUsuario", "Este nombre de usuario ya está en uso, intenta con otro.");
            }
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Ocurrió un error inesperado al registrar tu usuario, intente de nuevo más tarde.");
				_logger.LogError(ex, "Error inesperado al intentar registrarse.");
			}
            
            return View(usuarioVM);
        }

        [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public IActionResult Modificar(int idUsuario)
        {
            try
            {
			    var usuario = _usuarioRepository.GetById(idUsuario);
                
                if(usuario is null)
                {
                    ViewData["Mensaje"] = "El usuario seleccionado no existe en nuestra base de datos."
                    return View("NotFound");
                }

                var usuarioVM = new ModificarUsuarioViewModel(usuario);
                return View(usuarioVM);
			}
			catch (Exception ex)
			{
				TempData["Mensaje"] = "Ocurrió un error inesperado al intentar modificar su usuario, intente de nuevo más tarde.";
				_logger.LogError(ex, "Error inesperado al intentar cambiar de contraseña.");
			}

            return RedirectToAction("Listar", "Tablero", new { idUsuario });
        }

        [HttpPost]
        [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public IActionResult Modificar(ModificarUsuarioViewModel usuarioVM)
        {
            try
            {
			    var usuario = new Usuario(usuarioVM);
                _usuarioRepository.Update(usuario);
                _authenticationService.ChangeUserName(usuario.NombreUsuario);

                TempData["Mensaje"] = "El usuario fue modificado con éxito.";
			}
            catch(MySqlException ex) when(ex.Number == 1062) //clave duplicada
            {
				ModelState.AddModelError("NombreUsuario", "Este nombre de usuario ya está en uso, intenta con otro.");
			}
			catch (Exception ex)
			{
				TempData["Mensaje"] = "Ocurrió un error inesperado al intentar modificar su usuario, intente de nuevo más tarde.";
				_logger.LogError(ex, "Error inesperado al intentar cambiar de contraseña.");
			}
            
            return RedirectToAction("Modificar", new {idUsuario = usuarioVM.Id});
        }


        public IActionResult EliminarPropio(int idUsuario)
        {
            var usuarioVM = new EliminarUsuarioViewModel(idUsuario);
            return View(usuarioVM);
        }

        [HttpPost]
        public IActionResult EliminarPropio(EliminarUsuarioViewModel usuarioVM)
        {
            try
            {
                var sonIguales = VerificarContrasenia(usuarioVM.Id, usuarioVM.ContraseniaIngresada);

                if (sonIguales)
                {
                    _usuarioRepository.Delete(usuarioVM.Id);
                    TempData["Mensaje"] = "El usuario fue eliminado correctamente.";
                    return RedirectToAction("Logout", "Login");
                }
                usuarioVM.ErrorMessage = "Las contraseñas no coinciden, asegúrese de escribir todo correctamente.";

            } catch(MySqlException ex) when(ex.Number == 1451) //error de restriccion de FK
            {
                ModelState.AddModelError("", "Este usuario está relacionado con tareas y/o tableros, por lo que no puede ser eliminado.");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Ocurrió un error inesperado al intentar eliminar su usuario, intente de nuevo más tarde.");
				_logger.LogError(ex, "Error inesperado al intentar eliminar usuario propio");
			}

			return View(usuarioVM);
        }

        [AccessLevel(RolUsuario.Admin)]
		public IActionResult EliminarParaAdmin(int idUsuario)
		{
            try
            {
			    _usuarioRepository.Delete(idUsuario);
                TempData["Mensaje"] = "El usuario fue eliminado correctamente.";
		    }
			catch (MySqlException ex) when (ex.Number == 1451) //error de restriccion de FK
			{
				ModelState.AddModelError("", "El usuario que se quiere borrar está relacionado con tareas y/o tableros, por lo que no puede ser eliminado.");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Ocurrió un error inesperado al intentar eliminar su usuario, intente de nuevo más tarde.");
				_logger.LogError(ex, "Error inesperado al intentar eliminar usuario propio");
			}

			return RedirectToAction("Editar");
        }
	    
		[AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public IActionResult CambiarContra(int idUsuario)
        {
            var usuarioVM = new CambiarContraViewModel(idUsuario);
     
            return View(usuarioVM);
        }

        [HttpPost]
        [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public IActionResult CambiarContra(CambiarContraViewModel usuarioVM)
        {
            try
            {
                bool sonIguales = VerificarContrasenia(usuarioVM.IdUsuario, usuarioVM.PasswordActualInput);
			    if (sonIguales)
			    {
				    _usuarioRepository.ChangePassword(usuarioVM.IdUsuario, _passwordService.HashPassword(usuarioVM.PasswordNueva));

                    TempData["Mensaje"] = "La nueva contraseña ha sido guardada con éxito. Por favor, inicie sesión de vuelta.";
                    return RedirectToAction("Logout", "Login");
                }

                usuarioVM.ErrorMessage = "La contraseña ingresada no coincide con la actual, asegúrate de escribirla correctamente";
            }
            catch (Exception ex)
            {
				ModelState.AddModelError("", "Ocurrió un error inesperado, intente de nuevo más tarde.");
				_logger.LogError(ex, "Error inesperado al intentar cambiar de contraseña.");
			}

            return View(usuarioVM);
        }

        [AccessLevel(RolUsuario.Admin)]
        public IActionResult Crear()
        {
            return View(new CrearUsuarioViewModel());
        }

        [HttpPost]
        [AccessLevel(RolUsuario.Admin)]
        public IActionResult Crear(CrearUsuarioViewModel usuarioVM)
        {
            try
            {
                var usuario = new Usuario(usuarioVM.NombreUsuario, usuarioVM.Email, _passwordService.HashPassword(usuarioVM.Contrasenia), usuarioVM.Rol);

			    _usuarioRepository.Create(usuario);
                TempData["Mensaje"] = "El nuevo usuario fue creado con éxito.";
			}
			catch (MySqlException ex) when (ex.Number == 1062) //clave duplicada
			{
				ModelState.AddModelError("NombreUsuario", "Este nombre de usuario ya está en uso, intenta con otro.");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Ocurrió un error inesperado, intente de nuevo más tarde.");
				_logger.LogError(ex, "Error inesperado al intentar crear un nuevo usuario.");
			}

			return RedirectToAction("Crear");
        }
        
        [AccessLevel(RolUsuario.Admin)]
        public IActionResult Editar()
        {
            return View(new List<EditarUsuarioViewModel>());
        }

        [AccessLevel(RolUsuario.Admin)]
        public IActionResult CambiarRol(int idUsuario)
        {
            try
            {
                var usuario = _usuarioRepository.GetById(idUsuario);

                if(usuario is null)
                {
                    ViewData["Mensaje"] = "El usuario solicitado no existe en nuestra base de datos.";
                    return View("NotFound");
                }

                var usuarioVM = new CambiarRolViewModel(usuario.NombreUsuario, idUsuario);
                return View(usuarioVM);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Ocurrió un error inesperado, intente de nuevo más tarde.");
				_logger.LogError(ex, "Error inesperado al intentar cambiar de rol a un usuario.");
			}

            return RedirectToAction("Editar");
		}

        [HttpPost]
        [AccessLevel(RolUsuario.Admin)]
        public IActionResult CambiarRol(CambiarRolViewModel usuarioVM)
        {
            try
            {
			    bool sonIguales = VerificarContrasenia(_authenticationService.GetUserId(), usuarioVM.ContraseniaIngresada);

			    if (sonIguales)
			    {
				    _usuarioRepository.ChangeRol(usuarioVM.Id, usuarioVM.Rol);

                    TempData["Mensaje"] = "El cambio de rol se ha producido de manera exitosa.";
                    return RedirectToAction("Editar");
                }

                usuarioVM.ErrorMessage = "Las contraseñas no coinciden, asegúrate de escribir todo correctamente";
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Ocurrió un error inesperado, intente de nuevo más tarde.");
				_logger.LogError(ex, "Error inesperado al cambiar de rol a un usuario.");
			}
            return View(usuarioVM);
        }

        [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public IActionResult Buscar(string nombreUsuario)
        {
            var usuariosBuscados = _usuarioRepository.SearchByName(nombreUsuario).ToList();
			
            if (!usuariosBuscados.Any())
			{
				TempData["Mensaje"] = $"No hemos encontrado coincidencias para '{nombreUsuario}', escribilo de otra forma y volvé a intentar.";
			}
			var usuariosVM = usuariosBuscados.Select(u => new EditarUsuarioViewModel(u.Id, u.NombreUsuario , u.Email, u.Rol)).ToList();

            return View("Editar", usuariosVM);
        }

        
    }
}
