using tl2_proyecto_2024_nachoNota.ViewModels.UsuarioVM;

namespace tl2_proyecto_2024_nachoNota.Models
{
    public class Usuario
    {
        private int id;
        private int idRol;
        private string nombreUsuario;
        private string password;
        private string email;

        public int Id { get; set; }
        public int IdRol { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public Usuario()
        {

        }

        public Usuario(ModificarUsuarioViewModel modificarUsuario)
        {
            Id = modificarUsuario.Id;
            NombreUsuario = modificarUsuario.NombreUsuario;
            Email = modificarUsuario.Email;
        }

        public Usuario(int id, int idRol, string nombreUsuario, string password, string email)
        {
            Id = id;
            IdRol = idRol;
            NombreUsuario = nombreUsuario;
            Password = password;
            Email = email;
        }
        public Usuario(int idRol, string nombreUsuario, string password, string email)
        {
            IdRol = idRol;
            NombreUsuario = nombreUsuario;
            Password = password;
            Email = email;
        }

        public Usuario(RegistrarUsuarioViewModel usuarioVM)
        {
            IdRol = 1;
            NombreUsuario = usuarioVM.NombreUsuario;
            Password = usuarioVM.Contrasenia;
            Email = usuarioVM.Email;
        }
    }
}
