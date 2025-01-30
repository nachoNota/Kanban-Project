using System.ComponentModel.DataAnnotations;
using tl2_proyecto_2024_nachoNota.Models;


namespace tl2_proyecto_2024_nachoNota.ViewModels.UsuarioVM
{
    public class ModificarUsuarioViewModel
    {
        [Required(ErrorMessage = "Se requiere un nombre de usuario.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "El nombre debe superar los 5 caracteres.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "Se requiere un mail.")]
        [EmailAddress(ErrorMessage = "Ingresar un mail válido.")]
        public string Email { get; set; }

        public int Id { get; set; }

        public ModificarUsuarioViewModel()
        {

        }

        public ModificarUsuarioViewModel(Usuario usuario)
        {
            Id = usuario.Id;
            NombreUsuario = usuario.NombreUsuario;
            Email = usuario.Email;
        }
    }
}
