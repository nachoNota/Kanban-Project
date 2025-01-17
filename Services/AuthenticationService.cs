using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Repositories;

namespace tl2_proyecto_2024_nachoNota.Services
{
    interface IAuthenticationService
    {
        bool Login(string nombreUsuario, string contrasenia, [FromServices] IRolRepository rolRepository);
        void Logout();
        bool IsAuthenticated();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpContext context;

        public AuthenticationService(IUsuarioRepository usuarioRepository, IHttpContextAccessor contextAccessor)
        {
            _usuarioRepository = usuarioRepository;
            _contextAccessor = contextAccessor;
            context = _contextAccessor.HttpContext;
        }
        public bool Login(string nombreUsuario, string contrasenia, [FromServices] IRolRepository rolRepository)
        {
            Usuario usuario = _usuarioRepository.GetUser(nombreUsuario, contrasenia);

            if (usuario is null) return false;

            context.Session.SetString("IsAuthenticated", "true");
            context.Session.SetString("User", nombreUsuario);
            Rol rol = rolRepository.GetById(usuario.IdRol);
            context.Session.SetString("AccessLevel", rol.ToString());

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
            context.Session.Remove("AccessLevel");
        }
    }
}
