using System.ComponentModel.DataAnnotations;

namespace tl2_proyecto_2024_nachoNota.ViewModels
{
    public class RecuperarPassViewModel
    {
        [Required(ErrorMessage = "Debe ingresar un mail para continuar.")]
        [EmailAddress(ErrorMessage = "El email ingresado debe ser válido.")]
        public string Email {  get; set; }
        
        [Required(ErrorMessage = "Debe ingresar un titulo para continuar.")]
        public string Subject { get; set; }

        [StringLength(80)]
        public string Body { get; set; }
    }
}
