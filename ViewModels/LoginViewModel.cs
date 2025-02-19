using System.ComponentModel.DataAnnotations;

namespace tl2_proyecto_2024_nachoNota.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debes ingresar un nombre de usuario.")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "Debes ingresar una contraseña.")]
        public string Contrasenia { get; set; }
        public bool IsAuthenticated {  get; set; }
        public string ErrorMessage { get; set; }
    }
}
