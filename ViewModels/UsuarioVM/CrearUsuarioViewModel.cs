using System.ComponentModel.DataAnnotations;
using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.ViewModels.UsuarioVM
{
    public class CrearUsuarioViewModel
    {
        [Required(ErrorMessage = "Debe asignarle un rol al usuario.")]
        public int IdRol { get; set; } = 0;

        [Required(ErrorMessage = "Se requiere un nombre de usuario.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "El usuario debe estar entre los 5 y 20 caracteres")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "Se requiere un mail.")]
        [EmailAddress(ErrorMessage = "Ingresar un mail válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Se requiere una contraseña.")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "La contraseña debe estar entre los 5 y 15 caracteres")]
        public string Contrasenia { get; set; }
        public List<Rol> Roles { get; set; }
        

        public CrearUsuarioViewModel() { }


        public CrearUsuarioViewModel(IEnumerable<Rol> roles)
        {
            Roles = roles.ToList();
        }
    }
}
