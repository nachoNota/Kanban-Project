using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Repositories;

namespace tl2_proyecto_2024_nachoNota.Services
{
    public interface IAuthenticationService
    {
        bool Login(string nombreUsuario, string contrasenia);
        void Logout();
        bool IsAuthenticated();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IRolRepository _rolRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpContext context;

        public AuthenticationService(IUsuarioRepository usuarioRepository, IHttpContextAccessor contextAccessor,
                                    IRolRepository rolRepository)
        {
            _usuarioRepository = usuarioRepository;
            _contextAccessor = contextAccessor;
            context = _contextAccessor.HttpContext;
            _rolRepository = rolRepository;
        }
        public bool Login(string nombreUsuario, string contrasenia)
        {
            Usuario usuario = _usuarioRepository.GetUser(nombreUsuario, contrasenia);

            if (usuario is null) return false;

            context.Session.SetString("IsAuthenticated", "true");
            context.Session.SetString("User", nombreUsuario);
            context.Session.SetInt32("IdUsuario", usuario.Id);
            Rol rol = _rolRepository.GetById(usuario.IdRol);
            context.Session.SetString("AccessLevel", rol.NombreRol);

            return true;
        }

        public bool IsAuthenticated()
        {
            var context = _contextAccessor.HttpContext;
            if (context is null)
                throw new InvalidOperationException("HttpContext no está disponible");

            return context.Session.GetString("IsAuthenticated") == "true";
        }


        public void Logout()
        {
            context.Session.Remove("IsAuthenticated");
            context.Session.Remove("User");
            context.Session.Remove("IdUsuario");
            context.Session.Remove("AccessLevel");
        }
    }
}
