using tl2_proyecto_2024_nachoNota.ViewModels.UsuarioVM;

namespace tl2_proyecto_2024_nachoNota.Models
{
    public class Usuario
    {
        private int id;
        private string nombreUsuario;
        private string password;
        private string email;
        private RolUsuario rol;

        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public RolUsuario Rol { get; set; }

        public Usuario()
        {

        }

        public Usuario(ModificarUsuarioViewModel modificarUsuario)
        {
            Id = modificarUsuario.Id;
            NombreUsuario = modificarUsuario.NombreUsuario;
            Email = modificarUsuario.Email;
        }

        public Usuario(int id, RolUsuario rol, string nombreUsuario, string password, string email)
        {
            Id = id;
            Rol = rol;
            NombreUsuario = nombreUsuario;
            Password = password;
            Email = email;
        }
        public Usuario(RolUsuario rol, string nombreUsuario, string password, string email)
        {
            Rol = rol;
            NombreUsuario = nombreUsuario;
            Password = password;
            Email = email;
        }

        public Usuario(RegistrarUsuarioViewModel usuarioVM)
        {
            NombreUsuario = usuarioVM.NombreUsuario;
            Password = usuarioVM.Contrasenia;
            Email = usuarioVM.Email;
        }
    }
}
