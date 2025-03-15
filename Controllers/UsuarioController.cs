using Microsoft.AspNetCore.Identity;
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

        private bool VerificarContrasenia(string contraseniaActual, string contraseniaIngresada)
        {
            bool sonIguales = _passwordService.VerifyPassword(contraseniaActual, contraseniaIngresada);
            return sonIguales;
        }

        public ActionResult Registrar()
        {
            var usuarioVM = new RegistrarUsuarioViewModel();
            return View(usuarioVM);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(RegistrarUsuarioViewModel usuarioVM)
        {
            try
            {
                var usuario = new Usuario
                {
                    NombreUsuario = usuarioVM.NombreUsuario,
                    Email = usuarioVM.Email,
                    Password = _passwordService.HashPassword(usuarioVM.Contrasenia),
                    Rol = RolUsuario.Operador
                };

                await _usuarioRepository.Create(usuario);
                TempData["Mensaje"] = "Usuario creado con éxito, prueba a iniciar sesión para confirmar los cambios.";
                return RedirectToAction("Index", "Login");
			}
            catch(MySqlException ex) when (ex.Number == 1062) // clave duplicada
            {
                ModelState.AddModelError("NombreUsuario", "Este nombre de usuario ya está en uso, intenta con otro.");
                return View(usuarioVM);
            }
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error inesperado al intentar registrarse.");
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar registar su usuario. Por favor, intente de nuevo más tarde." });
            }

        }

        [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public async Task<IActionResult> Modificar(int idUsuario)
        {
            try
            {
			    var usuario = await _usuarioRepository.GetById(idUsuario);
                var usuarioVM = new ModificarUsuarioViewModel
                {
                    NombreUsuario = usuario.NombreUsuario,
                    Email = usuario.Email,
                    Id = usuario.Id
                };

                return View(usuarioVM);
			}
            catch (KeyNotFoundException)
            {
                return RedirectToAction("NoEncontrado", "Error", new { mensaje = "El usuario solicitado no existe en nuestra base de datos." });
            }
            catch (Exception ex)
			{
                _logger.LogError(ex, "Error inesperado al intentar modificar al usuario {Id}.", idUsuario);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder su usuario. Por favor, intente de nuevo más tarde." });
            }
        }

        [HttpPost]
        [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public async Task<IActionResult> Modificar(ModificarUsuarioViewModel usuarioVM)
        {
            try
            {
                var usuario = new Usuario
                {
                    Email = usuarioVM.Email,
                    Id = usuarioVM.Id,
                    NombreUsuario = usuarioVM.NombreUsuario
                };
                
                await _usuarioRepository.Update(usuario);
                _authenticationService.ChangeUserName(usuario.NombreUsuario);

                TempData["Mensaje"] = "El usuario fue modificado con éxito.";
                return RedirectToAction("Modificar", new {idUsuario = usuarioVM.Id});
			}
            catch(MySqlException ex) when(ex.Number == 1062) //clave duplicada
            {
				ModelState.AddModelError("NombreUsuario", "Este nombre de usuario ya está en uso, intenta con otro.");
                return View(usuarioVM); 
			}
			catch (Exception ex)
			{
                _logger.LogError(ex, "Error inesperado al intentar modificar el usuario {Id}.", usuarioVM.Id);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar modificar su usuario. Por favor, intente de nuevo más tarde." });
			}
        }


        public async Task<IActionResult> EliminarPropio(int idUsuario)
        {
            try
            {
                var usuario = await _usuarioRepository.GetById(idUsuario);
                var usuarioVM = new EliminarUsuarioViewModel(idUsuario);

                return View(usuarioVM);
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction("NoEncontrado", "Error", new { mensaje = "El usuario solicitado no existe en nuestra base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar acceder al usuario {Id}.", idUsuario);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder a su usuario. Por favor, intente de nuevo más tarde." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarPropio(EliminarUsuarioViewModel usuarioVM)
        {
            try
            {
                string passwordActual = await _usuarioRepository.GetPasswordById(usuarioVM.Id);
                var sonIguales = VerificarContrasenia(passwordActual, usuarioVM.ContraseniaIngresada);

                if (sonIguales)
                {
                    await _usuarioRepository.Delete(usuarioVM.Id);
                    TempData["Mensaje"] = "El usuario fue eliminado correctamente.";
                    return RedirectToAction("Logout", "Login");
                }

                usuarioVM.ErrorMessage = "Las contraseñas no coinciden, asegúrese de escribir todo correctamente.";
            }
            catch(MySqlException ex) when(ex.Number == 1451) //error de restriccion de FK
            {
                ModelState.AddModelError("", "Este usuario está relacionado con tareas y/o tableros, por lo que no puede ser eliminado.");
			}
			catch (Exception ex)
			{
                _logger.LogError(ex, "Error inesperado al intentar eliminar el usuario {Id}.", usuarioVM.Id);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar eliminar su usuario. Por favor, intente de nuevo más tarde." });
			}
			
            return View(usuarioVM);
        }

        [AccessLevel(RolUsuario.Admin)]
		public async Task<IActionResult> EliminarParaAdmin(int idUsuario)
		{
            try
            {
			    await _usuarioRepository.Delete(idUsuario);
                TempData["Mensaje"] = "El usuario fue eliminado correctamente.";
		    }
			catch (MySqlException ex) when (ex.Number == 1451) //error de restriccion de FK
			{
				TempData["Error"] = "El usuario que se quiere borrar está relacionado con tareas y/o tableros, por lo que no puede ser eliminado.";
			}
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar acceder al usuario {Id}.", idUsuario);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder al usuario seleccionado. Por favor, intente de nuevo más tarde." });
            }

            return RedirectToAction("Editar");
        }
	    
		[AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public async Task<IActionResult> CambiarContra(int idUsuario)
        {
            try
            {
                var usuario = await _usuarioRepository.GetById(idUsuario);
                var usuarioVM = new CambiarContraViewModel(idUsuario);

                return View(usuarioVM);
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction("NoEncontrado", "Error", new { mensaje = "El usuario solicitado no existe en nuestra base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar acceder al usuario {Id}.", idUsuario);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder a su usuario. Por favor, intente de nuevo más tarde." });
            }
        }

        [HttpPost]
        [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public async Task<IActionResult> CambiarContra(CambiarContraViewModel usuarioVM)
        {
            try
            {
                string passwordActual = await _usuarioRepository.GetPasswordById(usuarioVM.IdUsuario);
                bool sonIguales = VerificarContrasenia(passwordActual, usuarioVM.PasswordActualInput);
			    if (sonIguales)
			    {
				    await _usuarioRepository.ChangePassword(usuarioVM.IdUsuario, _passwordService.HashPassword(usuarioVM.PasswordNueva));

                    TempData["Mensaje"] = "La nueva contraseña ha sido guardada con éxito. Por favor, inicie sesión de vuelta.";
                    return RedirectToAction("Logout", "Login");
                }

                usuarioVM.ErrorMessage = "La contraseña ingresada no coincide con la actual, asegúrate de escribirla correctamente";
                return View(usuarioVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar cambiar contraseña de usuario {Id}.", usuarioVM.IdUsuario);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar cambiar su contraseña. Por favor, intente de nuevo más tarde." });
            }
        }

        [AccessLevel(RolUsuario.Admin)]
        public IActionResult Crear()
        {
            return View(new CrearUsuarioViewModel());
        }

        [HttpPost]
        [AccessLevel(RolUsuario.Admin)]
        public async Task<IActionResult> Crear(CrearUsuarioViewModel usuarioVM)
        {
            try
            {
                var usuario = new Usuario
                {
                    NombreUsuario = usuarioVM.NombreUsuario,
                    Email = usuarioVM.Email,
                    Password = _passwordService.HashPassword(usuarioVM.Contrasenia),
                    Rol = usuarioVM.Rol
                };

			    await _usuarioRepository.Create(usuario);
                TempData["Mensaje"] = "El nuevo usuario fue creado con éxito.";
                return RedirectToAction("Crear");
			}
			catch (MySqlException ex) when (ex.Number == 1062) //clave duplicada
			{
				ModelState.AddModelError("NombreUsuario", "Este nombre de usuario ya está en uso, intenta con otro.");
                return View(usuarioVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar crear un nuevo usuario.");
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar crear el nuevo usuario. Por favor, intente de nuevo más tarde." });
            }

        }
        
        [AccessLevel(RolUsuario.Admin)]
        public IActionResult Editar()
        {
            return View(new List<EditarUsuarioViewModel>());
        }

        [AccessLevel(RolUsuario.Admin)]
        public async Task<IActionResult> CambiarRol(int idUsuario)
        {
            try
            {
                var usuario = await _usuarioRepository.GetById(idUsuario);
                var usuarioVM = new CambiarRolViewModel(usuario.NombreUsuario, idUsuario);

                return View(usuarioVM);
			}
            catch (KeyNotFoundException)
            {
                return RedirectToAction("NoEncontrado", "Error", new { mensaje = "El usuario solicitado no existe en nuestra base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar acceder al usuario {Id}.", idUsuario);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar acceder al usuario seleccionado. Por favor, intente de nuevo más tarde." });
            }
		}

        [HttpPost]
        [AccessLevel(RolUsuario.Admin)]
        public async Task<IActionResult> CambiarRol(CambiarRolViewModel usuarioVM)
        {
            try
            {
                string passwordActual = await _usuarioRepository.GetPasswordById(_authenticationService.GetUserId());
			    bool sonIguales = VerificarContrasenia(passwordActual, usuarioVM.ContraseniaIngresada);

			    if (sonIguales)
			    {
				    await _usuarioRepository.ChangeRol(usuarioVM.Id, usuarioVM.Rol);

                    TempData["Mensaje"] = "El cambio de rol se ha producido de manera exitosa.";
                    return RedirectToAction("Editar");
                }

                usuarioVM.ErrorMessage = "Las contraseñas no coinciden, asegúrate de escribir todo correctamente";
                return View(usuarioVM);
			}
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar cambiar rol al usuario {Id}.", usuarioVM.Id);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar cambiar de rol al usuario seleccionado. Por favor, intente de nuevo más tarde." });
            }
        }

        [AccessLevel(RolUsuario.Admin, RolUsuario.Operador)]
        public async Task<IActionResult> Buscar(string nombreUsuario)
        {
            try
            {
                var usuariosBuscados = await _usuarioRepository.SearchByName(nombreUsuario);
			
                if (!usuariosBuscados.Any())
			    {
				    TempData["Mensaje"] = $"No hemos encontrado coincidencias para '{nombreUsuario}', escribilo de otra forma y volvé a intentar.";
			    }

			    var usuariosVM = usuariosBuscados.Select(u => new EditarUsuarioViewModel(u.Id, u.NombreUsuario , u.Email, u.Rol)).ToList();
                return View("Editar", usuariosVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar buscar al usuario {nombreUsuario}.", nombreUsuario);
                return RedirectToAction("ErrorInesperado", "Error", new { mensaje = "Ocurrió un error inesperado al intentar buscar al usuario seleccionado. Por favor, intente de nuevo más tarde." });
            }
        }
    }
}
