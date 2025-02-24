using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Repositories;

namespace tl2_proyecto_2024_nachoNota.Services
{
    public interface IAuthenticationService
    {
        bool Login(string nombreUsuario, string contrasenia);
        void Logout();
        bool IsAuthenticated();
        void ChangeUserName(string nombreUsuario);
        public RolUsuario GetAccessLevel();
        public int GetUserId();

    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPasswordService _passwordService;
        private readonly HttpContext context;

        public AuthenticationService(IUsuarioRepository usuarioRepository, IHttpContextAccessor contextAccessor, IPasswordService passwordService)
        {
            _usuarioRepository = usuarioRepository;
            _contextAccessor = contextAccessor;
            context = _contextAccessor.HttpContext;
            _passwordService = passwordService;
        }
        public bool Login(string nombreUsuario, string contrasenia)
        {
            Usuario usuario = _usuarioRepository.GetByName(nombreUsuario);

            if (usuario is null) return false;

            if(_passwordService.VerifyPassword(usuario.Password, contrasenia))
            {
                context.Session.SetString("IsAuthenticated", "true");
                context.Session.SetString("User", nombreUsuario);
                context.Session.SetInt32("IdUser", usuario.Id);
                context.Session.SetString("AccessLevel", usuario.Rol.ToString());
                return true;
            }

            return false;
        }

        public void ChangeUserName(string nombreUsuario)
        {
            context.Session.SetString("User", nombreUsuario);
        }

        public RolUsuario GetAccessLevel()
        {
            var rol = ConvertToAccessLevel();

            return rol;
        }

        private RolUsuario ConvertToAccessLevel()
        {
            return (RolUsuario)Enum.Parse(typeof(RolUsuario), context.Session.GetString("AccessLevel"));
        }
		public int GetUserId()
		{
			return context.Session.GetInt32("IdUser").Value;
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
            context.Session.Remove("IdUser");
            context.Session.Remove("AccessLevel");
        }

	}
}
