using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace tl2_proyecto_2024_nachoNota.ViewModels
{
    public class RegistrarUsuarioViewModel
    {
        [Required(ErrorMessage = "Se requiere un nombre de usuario.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "El nombre debe superar los 5 caracteres.")]
        public string NombreUsuario { get; set; }
        
        [Required(ErrorMessage = "Se requiere un mail.")]
        [EmailAddress(ErrorMessage = "Ingresar un mail válido.")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Se requiere una contraseña.")]
        public string Contrasenia { get; set; }
    }
}
