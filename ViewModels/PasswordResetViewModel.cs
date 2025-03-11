using System.ComponentModel.DataAnnotations;

namespace tl2_proyecto_2024_nachoNota.ViewModels
{
    public class PasswordResetViewModel
    {
        [Required(ErrorMessage = "Debe ingresar una contraseña.")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "La contraseña debe estar entre los 5 y 15 caracteres")]
        public string PasswordNueva {  get; set; }

        [Required(ErrorMessage = "Debe volver a ingresar su contraseña.")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "La contraseña debe estar entre los 5 y 15 caracteres")]
        public string ConfirmPassword { get; set; }
    
        public string Token { get; set; }
        public string ErrorMessage {  get; set; }
    }
}
